using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Refahi.Notif.Domain.Core.Utility
{
    public static class JsonTools
    {
        //private static JsonSerializerOptions _options => new JsonSerializerOptions { 
        //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase ,            
        //};
        //public static string Serilize<T>(this T data) => JsonSerializer.Serialize(data, _options);
        //public static T DeSerilize<T>(this string data) => JsonSerializer.Deserialize<T>(data, _options);


        private static JsonSerializerSettings _options => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public static string Serilize<T>(this T data) => JsonConvert.SerializeObject(data, _options);
        public static T DeSerilize<T>(this string data) => JsonConvert.DeserializeObject<T>(data, _options);
    }
}
