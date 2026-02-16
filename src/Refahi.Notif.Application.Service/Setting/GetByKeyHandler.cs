using MediatR;
using Refahi.Notif.Application.Contract.Dtos.Setting.Queries;
using Refahi.Notif.Domain.Contract;
using Refahi.Notif.Domain.Core.Utility;

namespace Refahi.Notif.Application.Service.Setting
{
    public class GetByKeyHandler : IRequestHandler<GetByKeyRequest, GetByKeyResponse>
    {
        private ISettingsHolder settingsHolder;

        public GetByKeyHandler(ISettingsHolder settingsHolder)
        {
            this.settingsHolder = settingsHolder;
        }

        public async Task<GetByKeyResponse> Handle(GetByKeyRequest request, CancellationToken cancellationToken)
        {
            var model = settingsHolder.GetModel(request.Key);
            if (model == null) return null;
            return new GetByKeyResponse
            {
                Value = model.Value,
                Items = model.ValueType == null || !model.ValueType.ToLower().StartsWith("enum") ? null : GetItems(model.ValueType).ToList()
            };
        }
        private IEnumerable<SelectListItem>? GetItems(string? valueType)
        {
            var type = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).First(s => s.Name == valueType.Split(':')[1]);

            foreach (var value in Enum.GetValues(type))
            {
                yield return new SelectListItem
                {
                    Title = ((Enum)value).GetDisplayName(),
                    Value = ((int)Enum.Parse(type, value.ToString())).ToString()
                };
            }
        }
    }
}
