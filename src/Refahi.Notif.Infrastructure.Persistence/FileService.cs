using Minio;
using Minio.DataModel.Args;
using Refahi.Notif.Domain.Contract.Repositories;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Refahi.Notif.Infrastructure.Persistence;

public class FileService : IFileService
{
    private readonly IMinioClient _minioClient;
    private readonly MinioConfiguration _minioConfiguration;
    private const string FileNameKey = "filename";

    public FileService(MinioConfiguration minioConfiguration)
    {
        // Create an HttpClient with a timeout
        var httpClientHandler = new HttpClientHandler();
        var httpClient = new HttpClient(httpClientHandler)
        {
            Timeout = TimeSpan.FromMinutes(5) // Setting a higher timeout
        };
        _minioConfiguration = minioConfiguration;
        _minioClient = new MinioClient()
            .WithEndpoint(_minioConfiguration.Endpoint)
            .WithTimeout(60000) // Setting a higher timeout (60 seconds)
            .WithCredentials(_minioConfiguration.AccessKey, _minioConfiguration.SecretKey)
            .WithHttpClient(httpClient);

        if (_minioConfiguration.Protocol == "https")
            _minioClient.WithSSL();
        _minioClient.Build();
    }

    public async Task<string> Upload(string objectName, byte[] contents)
    {
        var objectId = Guid.NewGuid().ToString("N");
        await Upload(objectId, objectName, contents);
        return objectId;
    }

    public async Task Upload(string objectId, string objectName, byte[] contents)
    {
        MemoryStream stream = new MemoryStream();
        stream.Write(contents, 0, contents.Length);
        stream.Position = 0;
        await MakeBucketIfNotExist(_minioConfiguration.BucketName);
        // Upload a file to bucket.
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_minioConfiguration.BucketName)
            .WithObject(_minioConfiguration.DirectoryName + objectId)
        //.WithFileName(objectName)
            .WithObjectSize(stream.Length)
        //.WithContentType(fileContentType)
        .WithStreamData(stream);

        var jsonStringName = objectName;
        if (System.Text.Encoding.UTF8.GetByteCount(objectName) != objectName.Length)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            jsonStringName = JsonSerializer.Serialize(objectName, options);
        }

        var metaData = new Dictionary<string, string> { { FileNameKey, jsonStringName } };
        putObjectArgs.WithHeaders(metaData);
        var res = await _minioClient.PutObjectAsync(putObjectArgs);
        Console.WriteLine(res.ObjectName);
    }

    public async Task<MemoryStream> Download(string objectId)
    {
        var stream = new MemoryStream();
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(_minioConfiguration.BucketName)
            .WithObject(_minioConfiguration.DirectoryName + objectId)
            .WithCallbackStream(minioStream =>
            {
                minioStream.CopyTo(stream);
            });
        var objectStat = await _minioClient.GetObjectAsync(getObjectArgs);
        stream.Position = 0;
        return stream;
    }

    private async Task MakeBucketIfNotExist(string bucketName)
    {
        var beArgs = new BucketExistsArgs()
            .WithBucket(bucketName);
        bool found = await _minioClient.BucketExistsAsync(beArgs).ConfigureAwait(false);
        if (!found)
        {
            var mbArgs = new MakeBucketArgs()
                .WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(mbArgs).ConfigureAwait(false);
        }
    }

}