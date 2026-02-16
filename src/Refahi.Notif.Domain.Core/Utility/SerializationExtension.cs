using System.Text.Json;
using System.Text.Json.Serialization;

namespace Refahi.Notif.Domain.Core.Utility
{
    public static class SerializationExtension
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        public static string ToJson(this object value)
        {
            return JsonSerializer.Serialize(value, jsonSerializerOptions);
        }
        public static byte[] ToByteArray(this object obj)
        {

            return JsonSerializer.SerializeToUtf8Bytes(obj, jsonSerializerOptions);

        }

        public static T? FromByteArray<T>(this byte[] byteArray) where T : class
        {
            using var memStream = new MemoryStream(byteArray);

            memStream.Seek(0, SeekOrigin.Begin);
            return JsonSerializer.Deserialize<T>(memStream, jsonSerializerOptions);
        }
    }
}
