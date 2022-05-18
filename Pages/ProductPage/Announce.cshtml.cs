using Data;
using Entities;
using Inputs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace application.Pages.ProductPage;
public class AnnounceModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DataContext _context;

    private readonly IWebHostEnvironment _env;
    [BindProperty]
    public Product Product { get; set; } = new();

    public AnnounceModel(ILogger<IndexModel> logger, DataContext context, IWebHostEnvironment env)
    {
        _logger = logger;
        _context = context;
        _env = env;
    }

[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Size,Description,Images")] Product product ,IFormFile file)
    {
        if (ModelState.IsValid)
        {
            
            string uniqueFileName = null;  //to contain the filename
            if (file!= null)  //handle iformfile
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "Images");
                uniqueFileName =file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            product.Image = uniqueFileName; //fill the image property
            _context.Add(product);  //save
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return Page();
    }

    public async Task<IActionResult> Create()
    {
        var product = new Product
        {
            Size = Product.Size,
            Name = Product.Name,
            Color = Product.Color,
            Image = Product.Image,
            Price = Product.Price,
            Description = Product.Description,
            Created = DateTime.Now
        };    
       // Products = await _context.Products.ToListAsync();
        return Page();
    }

}