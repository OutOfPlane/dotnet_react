using Microsoft.AspNetCore.Mvc;
using Models;
using CustomExtensions;
using CustomAttributes;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("products")]
public class ProductEndpoint : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TemplateService _templateService;
    public ProductEndpoint(AppDbContext context, TemplateService templateService)
    {
        _context = context;
        _templateService = templateService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResult<Product>>> GetProducts([FromQuery] PaginationParams paging)
    {
        var products = await _context.Products.OrderBy(x => x.Id).PaginateAsync(paging);
        return products;
    }

    [HttpPost("filter")]
    public async Task<ActionResult<PagedResult<Product>>> FilterProducts([FromQuery] PaginationParams paging, [FromBody] FilterRule filter)
    {
        var products = await _context.Products.Where(filter.BuildPredicate<Product>()).OrderBy(x => x.Id).PaginateAsync(paging);
        return products;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpGet("render/{id}")]
    public async Task<IActionResult> GetRenderProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        var html = await _templateService.RenderTemplateAsync("Invoice", product);
        return Content(html, "text/html");
    }
}