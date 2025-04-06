using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Threading.Tasks;

public class S3Service
{
    private const string bucketName = "motosphere-images"; // Replace with your S3 bucket name
    private const string accessKey = "AKIAQERQQJW6YFRHJSNQ"; // Replace with IAM Access Key
    private const string secretKey = "o3Kr1MCUy7D6z80AhuWqfJVhJegQMGltyI4sex6q"; // Replace with IAM Secret Key
    private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUNorth1;

    private readonly IAmazonS3 _s3Client;

    public S3Service()
    {
        _s3Client = new AmazonS3Client(accessKey, secretKey, bucketRegion);
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            BucketName = bucketName,
            Key = fileName,
            ContentType = "image/jpeg" // Adjust based on file type if needed
        };

        var fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return $"https://{bucketName}.s3.{bucketRegion.SystemName}.amazonaws.com/{fileName}";
    }

    public async Task DeleteFileAsync(string fileKey)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = fileKey
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
    }

}
