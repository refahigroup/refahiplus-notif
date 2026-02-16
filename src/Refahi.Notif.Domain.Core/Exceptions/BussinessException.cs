namespace Refahi.Notif.Domain.Core.Exceptions
{
    public class BussinessException : Exception
    {
        public BussinessException(IEnumerable<string> messages) : base(string.Join(",", messages)) { }
        public BussinessException(string message) : base(message) { }
    }
}
