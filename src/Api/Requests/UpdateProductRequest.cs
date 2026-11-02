using Application.Features.Management.Products.Dtos;

namespace Api.Requests;

public sealed record UpdateProductRequest
{

    public Guid BrandId {get; set;}
    public long CategoryId {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public bool IsSerialized {get; set;}
    public decimal Price {get; set;}
    public int Width {get; set;}
    public int Height{get; set;}
    public int Length{get; set;}
    public int Weight{get; set;}
    public string Sku{get; set;}
    public string BarCode{get; set;}
    public string Slug{get; set;}

// Accept them as raw strings from FormData
    public string? ImagesDetailsJson { get; set; }
    public string? Specifications { get; set; }
    public List<IFormFile>? ImagesFiles { get; set; }
    public InventoryDto? Inventory {get; set;}
}
