
using Application.Common.Enums;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.UrlProviders;

internal sealed class ProductThumbnailUrlProvider : IProductThumbnailUrlProvider
{
    private readonly MediaOptions _media;
    private readonly IApplicationUrlProvider _urlProvider;

    public ProductThumbnailUrlProvider(IOptions<MediaOptions> options, IApplicationUrlProvider urlProvider)
    {
        _media = options.Value;
        _urlProvider = urlProvider;
    }

    public string GetDefaultThumbnailUrl(ProductThumbnailSize size)
    {
        var path = size switch
        {
            ProductThumbnailSize.Small => _media.Images.Products.DefaultThumbnailSmall,
            ProductThumbnailSize.Medium => _media.Images.Products.DefaultThumbnailMedium,
            ProductThumbnailSize.Large => _media.Images.Products.DefaultThumbnailLarge,
            _ => throw new ArgumentOutOfRangeException(nameof(size))
        };
        return $"{path}";
    }

    public string GetRelativeUrl(string imageName, ProductThumbnailSize size)
    {
        if (imageName is null)
        {
            return GetDefaultThumbnailUrl(size);
        }

        var path = GetSizePath(size);

        return $"/{path}/{imageName}";
    }
    public string GetAbsoluteUrl(string imageName, ProductThumbnailSize size)
    {
        if (imageName is null)
        {
            return GetDefaultThumbnailUrl(size);
        }

        var path = GetSizePath(size);

        return $"{_urlProvider.BaseUrl}/{path}/{imageName}";
    }

    private string GetSizePath(ProductThumbnailSize size)
    => size switch
    {
        ProductThumbnailSize.Small => _media.Images.Products.Small,
        ProductThumbnailSize.Medium => _media.Images.Products.Medium,
        ProductThumbnailSize.Large => _media.Images.Products.Large,
        _ => throw new ArgumentOutOfRangeException(nameof(size))
    };
}
