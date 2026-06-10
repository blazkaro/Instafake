using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace Instafake.Multimedia;

[Route("")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MultimediaController(IAmazonS3 s3Client, IOptions<StorageConfig> storageConfig) : ControllerBase
{
    private const int MAX_BYTES_SIZE = 1024 * 1024 * 20;
    private readonly ImmutableDictionary<string, string[]> ALLOWED_MEDIA_TYPES = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
    {
        {"image/jpeg", ["jpg", "jpeg"]},
        {"image/png", ["png"] },
        {"image/webp", ["webp"] },
        {"image/gif", ["gif"] },
        {"image/avif", ["avif"] },
        {"video/mp4", ["mp4"] },
        {"video/webm", ["webm"] },
        {"video/ogg", ["ogg", "ogv"] }
    }.ToImmutableDictionary();

    private readonly IAmazonS3 _s3Client = s3Client;
    private readonly StorageConfig _storageConfig = storageConfig.Value;

    [HttpPost("posts")]
    public async Task<IActionResult> Posts([FromBody][MaxLength(10)] List<FileMetadataDto> fileMetadataDtos)
    {
        if (fileMetadataDtos.Sum(metadata => metadata.SizeBytes) > MAX_BYTES_SIZE)
            return BadRequest($"Maximum allowed cumulative files size is {MAX_BYTES_SIZE / 1024}MiB");

        if (fileMetadataDtos.Any(metadata => !ALLOWED_MEDIA_TYPES.ContainsKey(metadata.ContentType) || !ALLOWED_MEDIA_TYPES[metadata.ContentType].Contains(metadata.Extension)))
            return BadRequest($"Disallowed MIME or MIME/extension mismatch");

        var uploadMetadatas = await CreateUploadMetadata(_storageConfig.Buckets.PostMultimedia, fileMetadataDtos);
        return Ok(uploadMetadatas);
    }

    private async Task<UploadMetadataDto> CreateUploadMetadata(GetPreSignedUrlRequest request)
    {
        return new()
        {
            UploadUrl = new Uri(await _s3Client.GetPreSignedURLAsync(request)),
            PublicUrl = new Uri($"{_storageConfig.PublicUrl}{_storageConfig.Buckets.PostMultimedia}/{request.Key}")
        };
    }

    private async Task<UploadMetadataDto[]> CreateUploadMetadata(string bucketName, List<FileMetadataDto> fileMetadataDtos)
    {
        var tasks = new List<Task<UploadMetadataDto>>(fileMetadataDtos.Count);
        foreach (var metadata in fileMetadataDtos)
        {
            var req = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = $"posts/{Guid.NewGuid()}{metadata.Extension}",
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(3),
                ContentType = metadata.ContentType,
                Protocol = Protocol.HTTP
            };

            tasks.Add(CreateUploadMetadata(req));
        }

        return await Task.WhenAll(tasks);
    }
}
