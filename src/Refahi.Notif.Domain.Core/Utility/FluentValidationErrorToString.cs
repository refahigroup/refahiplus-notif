using FluentValidation.Results;

namespace Refahi.Notif.Domain.Core.Utility
{
    public static class FluentValidationErrorToString
    {
        public static string JoinErrorsToString(this ValidationResult error) =>
            string.Join("\n", error.Errors.Select(x => x.ErrorMessage));
    }
}
