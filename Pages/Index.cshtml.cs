using Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Data;
using Microsoft.EntityFrameworkCore;

namespace application.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DataContext _context;

    public IndexModel(ILogger<IndexModel> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public Product Product { get; set; } = new();
    public IEnumerable<Product> Products {get;set; }
    public string Message { get; private set; } = "PageModel in C#";
    public async Task<IActionResult> OnGet()
    {
        Products = await _context.Products.ToListAsync();
        return Page();
    }

    public async Task<IActionResult> AddProduct()
    {
        /* sprawdz autentykacje użytkownika
        if (!Auth())
        {
         return new RedirectToPageResult("/Portal/Login");
        }
        return Page();
        */
        string url = HttpContext.Request.Path;  

        return Redirect(url);
    }
}
