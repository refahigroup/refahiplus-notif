using Refahi.Notif.Domain.Contract.Models;

namespace Refahi.Notif.Domain.Contract
{
    public interface ISettingsHolder
    {
        SettingModel? GetModel(SettingKey key);
        object? Get(SettingKey key);
        void Set(SettingKey key, string value);
        void Set(SettingKey key, string valueType, string value);
    }
}
