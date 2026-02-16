using Newtonsoft.Json;

namespace Refahi.Notif.Infrastructure.Messaging.PushNotification.Firebase.Model;

internal record FirebaseTokenResponse(
    [property: JsonProperty("access_token")] string AccessToken,
    [property: JsonProperty("token_type")] string TokenType,
    [property: JsonProperty("expires_in")] int ExpiresIn);