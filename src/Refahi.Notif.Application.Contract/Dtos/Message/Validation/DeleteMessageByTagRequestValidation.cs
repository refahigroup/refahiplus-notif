using FluentValidation;
using Refahi.Notif.Application.Contract.Dtos.Message.Commands;
using Refahi.Notif.Domain.Core.Exceptions;

namespace Refahi.Notif.Application.Contract.Dtos.Message.Validation
{

    public sealed class DeleteMessageByTagRequestValidation : AbstractValidator<DeleteMessageByTagRequest>
    {
        public DeleteMessageByTagRequestValidation()
        {
            RuleFor(x => x.Tag).NotEmpty().WithMessage(Errors.TagRequired);

        }
    }
}
