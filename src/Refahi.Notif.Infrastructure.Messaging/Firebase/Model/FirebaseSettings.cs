using Newtonsoft.Json;

namespace Refahi.Notif.Infrastructure.Messaging.PushNotification.Firebase.Model;

public record FirebaseSettings(
    [property: JsonProperty("project_id")] string ProjectId,
    [property: JsonProperty("private_key")] string PrivateKey,
    [property: JsonProperty("client_email")] string ClientEmail,
    [property: JsonProperty("token_uri")] string TokenUri
);