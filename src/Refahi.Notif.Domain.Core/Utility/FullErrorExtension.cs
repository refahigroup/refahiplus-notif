namespace Refahi.Notif.Domain.Core.Utility
{
    public static class FullErrorExtension
    {
        public static string GetFullError(this Exception ex)
        {
            var error = ex.Message;
            while (ex.InnerException != null)
            {
                error += ex.InnerException.Message;
                ex = ex.InnerException;
            }
            return error;
        }
    }
}
