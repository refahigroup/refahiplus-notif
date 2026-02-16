namespace Refahi.Notif.Domain.Contract.Models
{
    public class SettingModel
    {
        public SettingKey Key { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }

    }
    public enum SettingKey
    {
        DefaultSmsGateway = 1
    }
}
