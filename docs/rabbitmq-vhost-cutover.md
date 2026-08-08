# RabbitMQ vhost cutover for branded Notification instances

`prod_notif` and `tochal_notif` must not consume from the same RabbitMQ virtual host. Separate users do not isolate exchanges or queues when both users have permissions on `/`.

## Required runtime configuration

The application refuses to start in `Production` when `BrokerInfo__VirtualHost` is missing or blank.

```text
prod_notif:
  BrokerInfo__Host=infra_rabbitmq
  BrokerInfo__VirtualHost=refahi-prod
  MedianaSmsConfiguration__OtpPatternCode=810889

tochal_notif:
  BrokerInfo__Host=infra_rabbitmq
  BrokerInfo__VirtualHost=tochal-prod
  MedianaSmsConfiguration__OtpPatternCode=812480
```

Keep usernames, passwords, API keys, and database credentials in the deployment secret store. Never paste them into this document or commit them to source control.

## Broker preparation

Back up definitions and capture the current state before making changes:

```bash
docker exec infra_rabbitmq rabbitmqctl export_definitions /tmp/definitions-before-vhost-cutover.json
docker cp infra_rabbitmq:/tmp/definitions-before-vhost-cutover.json ./definitions-before-vhost-cutover.json
docker exec infra_rabbitmq rabbitmqctl list_permissions -p /
docker exec infra_rabbitmq rabbitmqctl list_queues -p / name consumers messages_ready messages_unacknowledged
docker exec infra_rabbitmq rabbitmqctl list_consumers -p /
```

Create isolated vhosts and grant the existing service users access only to their target vhost:

```bash
docker exec infra_rabbitmq rabbitmqctl add_vhost refahi-prod
docker exec infra_rabbitmq rabbitmqctl add_vhost tochal-prod
docker exec infra_rabbitmq rabbitmqctl set_permissions -p refahi-prod notif-usr '.*' '.*' '.*'
docker exec infra_rabbitmq rabbitmqctl set_permissions -p tochal-prod tochal-usr '.*' '.*' '.*'
```

## Coordinated cutover

1. Pause all producers that publish Notification messages for both brands.
2. Wait until all queues on `/` report both `messages_ready=0` and `messages_unacknowledged=0`.
3. Deploy the vhost-aware Notification image.
4. Add the runtime configuration above to the corresponding compose service and restart both Notification containers.
5. Resume producers only after both vhosts show the expected queues and consumers.

Do not move legacy queued messages between vhosts. Existing messages do not carry a reliable brand identifier and cannot be separated safely.

## Verification

```bash
docker exec infra_rabbitmq rabbitmqctl list_queues -p refahi-prod name consumers messages_ready messages_unacknowledged
docker exec infra_rabbitmq rabbitmqctl list_queues -p tochal-prod name consumers messages_ready messages_unacknowledged
docker exec infra_rabbitmq rabbitmqctl list_consumers -p refahi-prod
docker exec infra_rabbitmq rabbitmqctl list_consumers -p tochal-prod
docker logs --since 10m prod_notif 2>&1 | grep 'Configuring RabbitMQ transport'
docker logs --since 10m tochal_notif 2>&1 | grep 'Configuring RabbitMQ transport'
```

Send controlled OTP requests to approved test numbers from both domains and verify the Mediana pattern IDs. Browser or User-Agent changes must not affect the selected pattern.

## Removing old access

First confirm that no other process uses either service user on `/`:

```bash
docker exec infra_rabbitmq rabbitmqctl list_user_permissions notif-usr
docker exec infra_rabbitmq rabbitmqctl list_user_permissions tochal-usr
docker exec infra_rabbitmq rabbitmqctl list_connections user vhost peer_host peer_port client_properties
```

Only after the audit and observation period, remove access to `/`:

```bash
docker exec infra_rabbitmq rabbitmqctl clear_permissions -p / notif-usr
docker exec infra_rabbitmq rabbitmqctl clear_permissions -p / tochal-usr
```

Keep the old topology for the agreed rollback window. Delete orphaned queues and exchanges only after a second backup and explicit operational approval.

## Rollback

An old application image hardcodes `/` and is not a safe rollback target on the shared broker. Prefer roll-forward. If an emergency rollback is unavoidable, point the old image at a temporary dedicated RabbitMQ container so it cannot rejoin the shared topology.
