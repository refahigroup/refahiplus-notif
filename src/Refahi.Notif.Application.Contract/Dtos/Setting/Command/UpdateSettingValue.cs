using MediatR;
using Refahi.Notif.Domain.Contract.Models;

namespace Refahi.Notif.Application.Contract.Dtos.Setting.Command
{
    public class UpdateSettingValueRequest : IRequest<Unit>
    {
        public SettingKey Key { get; set; }
        public string Value { get; set; }
    }
}
