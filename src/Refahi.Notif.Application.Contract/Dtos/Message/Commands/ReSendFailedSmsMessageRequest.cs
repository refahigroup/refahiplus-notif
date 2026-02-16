using MediatR;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Commands
{
    public class ReSendFailedSmsMessageRequest : IRequest<int>
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int MinRetryCount { get; set; }
        public Guid[]? Ids { get; set; }
    }
}
