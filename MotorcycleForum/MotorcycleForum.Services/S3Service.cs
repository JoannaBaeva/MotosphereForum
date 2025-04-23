using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

public class S3Service
{
    private readonly string bucketName;
    private readonly RegionEndpoint region;
    private readonly IAmazonS3 s3Client;

    public S3Service(string accessKey, string secretKey, string bucketName, string regionName)
    {
        this.bucketName = bucketName;
        this.region = RegionEndpoint.GetBySystemName(regionName);
        this.s3Client = new AmazonS3Client(accessKey, secretKey, this.region);
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            BucketName = bucketName,
            Key = fileName,
            ContentType = "image/jpeg"
        };

        var fileTransferUtility = new TransferUtility(s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return $"https://{bucketName}.s3.{region.SystemName}.amazonaws.com/{fileName}";
    }

    public async Task DeleteFileAsync(string fileKey)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = fileKey
        };

        await s3Client.DeleteObjectAsync(deleteRequest);
    }
}