namespace Refahi.Notif.Domain.Contract.Repositories
{
    public interface IFileService
    {
        Task<string> Upload(string objectName, byte[] contents);
        Task Upload(string objectId, string objectName, byte[] contents);
        Task<MemoryStream> Download(string fileNameId);
    }
}
