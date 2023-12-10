using Microsoft.AspNetCore.Mvc;
using Mocassini.Models;

namespace Mocassini.Controllers;

[ApiController]
[Route("[controller]")]
public class BrandController : Controller
{
    private readonly ILogger<BrandController> _logger;
    private readonly MocassiniDbContext _dbContext;

    public BrandController(ILogger<BrandController> logger, MocassiniDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet(Name = "GetBrands")]
    public IEnumerable<BrandEntity> Get()
    {
        return _dbContext.Brands;
    }

    [HttpGet("{id}", Name = "GetBrand")]
    public BrandEntity Get(int id)
    {
        return _dbContext.Brands.Find(id);
    }

    public class CreateBrandRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ExternalLogoId { get; set; } = null!;
    }

    [HttpPost(Name = "CreateBrand")]
    public BrandEntity Create([FromBody] CreateBrandRequest req)
    {
        var brand = _dbContext.Brands.Add(
            new BrandEntity
            {
                Name = req.Name,
                Description = req.Description,
                ExternalLogoId = req.ExternalLogoId
            }
        );
        _dbContext.SaveChanges();
        return brand.Entity;
    }

    [HttpDelete("{id}", Name = "DeleteBrand")]
    public void Delete(int id)
    {
        var brand = _dbContext.Brands.Find(id);
        _dbContext.Brands.Remove(brand);
        _dbContext.SaveChanges();
    }
}