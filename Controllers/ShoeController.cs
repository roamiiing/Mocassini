using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mocassini.Models;

namespace Mocassini.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoeController : Controller
{
    private readonly ILogger<ShoeController> _logger;
    private readonly MocassiniDbContext _dbContext;

    public ShoeController(ILogger<ShoeController> logger, MocassiniDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public class GetFilters
    {
        public string? Name { get; set; }
        public int? BrandId { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public string? VariantName { get; set; }
    }

    [HttpGet(Name = "GetShoes")]
    public IEnumerable<ShoeEntity> Get([FromQuery] GetFilters? filters)
    {
        filters ??= new GetFilters();

        return _dbContext.Shoes
            .Select(
                c => new ShoeEntity
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    BrandId = c.BrandId,
                    Price = c.Price,
                    Barcode = c.Barcode,

                    ShoeVarieties = c.ShoeVarieties.Select(sv => new ShoeVarietyEntity
                    {
                        Id = sv.Id,
                        Name = sv.Name,
                        Quantity = sv.Quantity,

                        ShoeVarietyPhotos = sv.ShoeVarietyPhotos.Select(
                            p => new ShoeVarietyPhotoEntity
                            {
                                Id = p.Id,
                                ExternalPhotoId = p.ExternalPhotoId
                            }
                        ).ToList()
                    }).ToList()
                }
            )
            .Where(
                c =>
                    (filters.Name == null || c.Name.Contains(filters.Name)) &&
                    (filters.BrandId == null || c.BrandId == filters.BrandId) &&
                    (filters.MinPrice == null || c.Price >= filters.MinPrice) &&
                    (filters.MaxPrice == null || c.Price <= filters.MaxPrice) &&
                    (
                        filters.VariantName == null ||
                        c.ShoeVarieties.Any(sv => sv.Name.Contains(filters.VariantName))
                    )
            )
            .ToList();
    }

    [HttpGet("{id}", Name = "GetShoe")]
    public ShoeEntity Get(int id)
    {
        return _dbContext.Shoes
            .Select(c => new ShoeEntity
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    BrandId = c.BrandId,
                    Price = c.Price,
                    Barcode = c.Barcode,
                    ShoeVarieties = c.ShoeVarieties.Select(sv => new ShoeVarietyEntity
                    {
                        Id = sv.Id,
                        Name = sv.Name,
                        Quantity = sv.Quantity,
                        ShoeVarietyPhotos = sv.ShoeVarietyPhotos.Select(
                            p => new ShoeVarietyPhotoEntity()
                            {
                                Id = p.Id,
                                ExternalPhotoId = p.ExternalPhotoId
                            }
                        ).ToList()
                    }).ToList()
                }
            )
            .First(c => c.Id == id);
    }

    public class CreateShoeRequest
    {
        public class ShoeVariety
        {
            public string Name { get; set; } = null!;
            public int Quantity { get; set; }
            public IEnumerable<string> Photos { get; set; } = null!;
        }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Barcode { get; set; } = null!;
        public int BrandId { get; set; }
        public int Price { get; set; }
        public IEnumerable<ShoeVariety> ShoeVarieties { get; set; } = null!;
    }

    [HttpPost(Name = "CreateShoe")]
    public ShoeEntity Create([FromBody] CreateShoeRequest req)
    {
        var shoe = new ShoeEntity
        {
            Name = req.Name,
            Description = req.Description,
            BrandId = req.BrandId,
            Price = req.Price,
            Barcode = req.Barcode,

            ShoeVarieties = req.ShoeVarieties.Select(sv => new ShoeVarietyEntity
            {
                Name = sv.Name,
                Quantity = sv.Quantity,
                ShoeVarietyPhotos = sv.Photos.Select(
                    p => new ShoeVarietyPhotoEntity()
                    {
                        ExternalPhotoId = p
                    }
                ).ToList()
            }).ToList()
        };

        _dbContext.Shoes.Add(shoe);
        _dbContext.SaveChanges();

        return shoe;
    }

    [HttpDelete("{id}", Name = "DeleteShoe")]
    public void Delete(int id)
    {
        var shoe = _dbContext.Shoes.Find(id);
        _dbContext.Shoes.Remove(shoe);
        _dbContext.SaveChanges();
    }
}