using MediatR;
using Refahi.Notif.Domain.Contract.Models;

namespace Refahi.Notif.Application.Contract.Dtos.Setting.Queries
{
    public class GetByKeyRequest : IRequest<GetByKeyResponse>
    {
        public SettingKey Key { get; set; }
    }
    public class GetByKeyResponse
    {
        public string? Value { get; set; }
        public ICollection<SelectListItem>? Items { get; set; }
    }
    public class SelectListItem
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
