using Microsoft.EntityFrameworkCore;
using Refahi.Notif.Domain.Contract;
using Refahi.Notif.Domain.Contract.Models;
using Refahi.Notif.Infrastructure.Persistence.Contract;

namespace Refahi.Notif.Infrastructure.Persistence
{

    public class SettingsHolder : ISettingsHolder
    {
        private static IDictionary<SettingKey, SettingModel>? settingsDic;
        private readonly IDbContext _notifContext;

        public SettingsHolder(IDbContext notifContext)
        {
            _notifContext = notifContext;
        }

        public SettingModel? GetModel(SettingKey key)
        {
            if (settingsDic == null)
                Fill();

            return settingsDic.ContainsKey(key) ? settingsDic[key] : null;

        }
        public object? Get(SettingKey key)
        {
            var value = GetModel(key);
            if (value == null)
                return null;

            return ConvertValue(value.ValueType, value.Value);
        }

        private object ConvertValue(string valueType, string value)
        {
            valueType = valueType.ToLower();

            switch (valueType)
            {
                case "int":
                    return int.Parse(value);
                case "string":
                    return value;
                default:
                    if (valueType.StartsWith("enum"))
                        return int.Parse(value);
                    throw new NotImplementedException();
            }
        }
        private void Fill()
        {
            settingsDic = null;
            settingsDic = _notifContext.Settings.ToDictionary(x => x.Key);
        }
        public void Set(SettingKey key, string value)
        {
            var setting = _notifContext.Settings.FirstOrDefault(x => x.Key == key);

            if (setting == null) throw new Exception("Setting NotFound");

            setting.Value = value.ToString();
            ((DbContext)_notifContext).Update(setting);
            _notifContext.SaveChanges();

            Fill();
        }
        public void Set(SettingKey key, string valueType, string value)
        {
            var setting = _notifContext.Settings.FirstOrDefault(x => x.Key == key);
            if (setting != null) return;

            setting = new SettingModel
            {
                Key = key,
                Value = value,
                ValueType = valueType
            };

            ((DbContext)_notifContext).Add(setting);

            _notifContext.SaveChanges();

        }
    }
}
