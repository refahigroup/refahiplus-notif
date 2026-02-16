using Newtonsoft.Json;

namespace Refahi.Notif.Infrastructure.Messaging.PushNotification.Firebase.Model;

public class FirebaseErrorResponse
{
    [JsonProperty("error")]
    public FirebaseError Error { get; set; }
}
public partial class FirebaseError
{
    [JsonProperty("code")]
    public long Code { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("details")]
    public FirebaseErrorDetail[] Details { get; set; }
}

public partial class FirebaseErrorDetail
{
    [JsonProperty("@type")]
    public string Type { get; set; }

    [JsonProperty("errorCode")]
    public string ErrorCode { get; set; }
}