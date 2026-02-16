using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Setting.Command;
using Refahi.Notif.Domain.Contract;

namespace Refahi.Notif.Application.Service.Setting
{
    public class UpdateSettingValueHandler : IRequestHandler<UpdateSettingValueRequest, Unit>
    {
        private readonly ISettingsHolder _settingsHolder;
        public UpdateSettingValueHandler(ISettingsHolder settingsHolder)
        {
            _settingsHolder = settingsHolder;
        }

        public async Task<Unit> Handle(UpdateSettingValueRequest request, CancellationToken cancellationToken)
        {
            _settingsHolder.Set(request.Key, request.Value);
            return Unit.Value;
        }
    }
}
