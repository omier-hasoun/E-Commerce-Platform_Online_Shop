using Domain.ProductGroups.Products;
using Domain.ProductGroups.ValueObjects;

namespace Application.Features.Management.Products.Commands.UpdateImages;

public sealed class UpdateImagesCommandHandler(IAppDbContext db, IImageStorageService imageStorage, IImageValidator imageValidator, IIdGenerator<long> idGenerator) : IRequestHandler<UpdateImagesCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(UpdateImagesCommand request, CancellationToken ct)
    {
        var productId = new ProductId(request.ProductId);
        var newImages = request.Images;
        var newFiles = request.NewFiles;

        var productGroup = await db.ProductGroups.Include(x => x.MainProduct)
                                                      .Where(x => x.MainProduct!.Id == productId)
                                                      .FirstOrDefaultAsync(ct);

        if(productGroup is null)
        {
            return ApplicationErrors.NotFound.Product;
        }

        // Comparing current Images Ids with the new Images Ids to determin
        // which ids are missing which then needs to be deleted from the server before overwriting the new Images to db

        var currentProductImagesList = productGroup.MainProduct!.Images.ToList();




        var incomingExistingImagesIds = newImages.Where(i => i.ExistingId.HasValue)
                                            .Select(i => i.ExistingId!.Value)
                                            .ToList();

        var newImagesList = newImages.Select(newImage =>
        {
            ProductImage image = null!;
            // get current image by id to take its fileName and Id and Update it with its new SortOrder
            var currImage = currentProductImagesList.FirstOrDefault(x => x.Id == newImage.ExistingId);
            if (currImage != null)
            {
                image = ProductImage.Create(currImage.Id, currImage.FileName, newImage.SortOrder);
            }
            else
            {
                var id = idGenerator.NewId();
                var fileName = newFiles[newImage.NewFileIndex!.Value].InternalFileName;

                image = ProductImage.Create(id, fileName, newImage.SortOrder);
            }

            return image;
        })
        .ToList();

        imageValidator.MaxSize = ApplicationRules.Uploads.MaxProductImageSize;
        imageValidator.MinHeight = ApplicationRules.Uploads.MinHeight;
        imageValidator.MinWidth = ApplicationRules.Uploads.MinWidth;

        if(newFiles != null && newFiles.Count > 0)
        {
            var validateResult = imageValidator.ValidateAll(newFiles);

            if (validateResult.Failed)
                return validateResult.Errors;

            var saveResult = await imageStorage.SaveAllAsync(newFiles, ct);

            if (saveResult.Failed)
                return saveResult.Errors;
        }

        productGroup.UpdateImages(productId, newImagesList);

        await db.SaveAsync(ct);

        var imagesToDelete = currentProductImagesList.Where(image => !incomingExistingImagesIds.Contains(image.Id))
                                                .Select(x => x.FileName)
                                                .ToList();
        if(imagesToDelete.Count > 0)
            imageStorage.DeleteAll(imagesToDelete);

        return Result.Success;
    }
}
