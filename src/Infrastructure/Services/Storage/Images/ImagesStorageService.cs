
using Application;
using Application.Common.Dtos;
using Infrastructure.Channels;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Storage.Images;

internal sealed class ImagesStorageService : IImageStorageService
{
    private readonly string _originalImagesDirPath;
    private readonly string _smallImagesPath;
    private readonly string _mediumImagesPath;
    private readonly string _largeImagesPath;
    private readonly IImageJobWriter _imageJobWriter;
    private readonly IFileStorageService _fileStore;
    public ImagesStorageService(IImageJobWriter imageJobWriter, IWebHostEnvironment webEnv, IFileStorageService fileStore, IOptions<MediaOptions> options)
    {
        _imageJobWriter = imageJobWriter;
        this._fileStore = fileStore;
        _originalImagesDirPath= Path.Combine(webEnv.WebRootPath, options.Value.Images.Products.Original);
        _smallImagesPath = Path.Combine(webEnv.WebRootPath, options.Value.Images.Products.Small);
        _mediumImagesPath = Path.Combine(webEnv.WebRootPath, options.Value.Images.Products.Medium);
        _largeImagesPath = Path.Combine(webEnv.WebRootPath, options.Value.Images.Products.Large);

    }

    public Result<Success> DeleteAll(List<string> fileNames)
    {
        List<string> filePaths = new(fileNames.Count * 3); // each file has 3 copies
        fileNames.ForEach(fileName =>
        {
            filePaths.Add(Path.Combine(_smallImagesPath, fileName));
            filePaths.Add(Path.Combine(_mediumImagesPath, fileName));
            filePaths.Add(Path.Combine(_largeImagesPath, fileName));

        });

        _fileStore.DeleteAllFiles(filePaths);
        return Result.Success;
    }

    public async ValueTask<Result<Success>> SaveAllAsync(IReadOnlyCollection<FileUploadDto> images, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(images);

        List<ImageProcessingJob> imageProcessJobs = new(images.Count);

        foreach (var image in images)
        {

            var filePath = Path.Combine(_originalImagesDirPath, image.InternalFileName);
            imageProcessJobs.Add(new ImageProcessingJob(filePath));

            if (await _fileStore.SaveAsync(image.ContentStream, filePath, ct) is false)
            {
                if (imageProcessJobs.Count > 0)
                {
                    List<string> savedImages = new(imageProcessJobs.Count);

                    imageProcessJobs.ForEach(x => savedImages.Add(x.FileName));

                    _fileStore.DeleteAllFiles(savedImages);
                }

                return ApplicationErrors.Unexpected.CouldntSaveImage;
            }

        }

        await _imageJobWriter.WriteAllAsync(imageProcessJobs);

        return Result.Success;
    }
}
