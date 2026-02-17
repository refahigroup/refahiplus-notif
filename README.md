"# Refahi.Notif Service

Notification service for Refahi Plus platform.

## Configuration

### MassTransit License

This service uses MassTransit v9 which requires a license configuration. You can configure the license in one of the following ways:

#### Option 1: Environment Variable (Recommended)
Set the `MT_LICENSE` environment variable:
```bash
export MT_LICENSE="your-license-key-here"
```

Or in Docker/Kubernetes:
```yaml
environment:
  - MT_LICENSE=your-license-key-here
```

#### Option 2: Configuration File
Add the license to `appsettings.json`:
```json
{
  "MassTransit": {
    "License": "your-license-key-here"
  }
}
```

#### Getting a License
- **Community License (Free)**: Visit [MassTransit Licensing](https://masstransit.io/pricing) to get a free community license
- **Commercial License**: For production deployments, consider purchasing a commercial license

If neither option is configured, the application will fail to start with a license error.
" 
