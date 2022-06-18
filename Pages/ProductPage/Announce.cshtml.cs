using Data;
using Entities;
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

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPost([Bind("Name,Price,Size,Color,Description,Image,Created")] Product product)
    {
        product = new Product
        {
            Id = Product.Id,
            Name = Product.Name,
            Price = Product.Price,
            Size = Product.Size,  
            Color = Product.Color,
            Description = Product.Description,
            Created = DateTime.Now            
        };  

        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();

            string wwrootPath =_env.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var savedProduct = _context.Products.Find(product.Id);
            if (savedProduct == null)
            {
                return Page();
            }

            if (files.Count !=0)
            {
                string imagePath = Path.Combine(_env.WebRootPath, "Images");
                var extension = Path.GetExtension(files[0].FileName);
                var relativeImagePath = imagePath + product.Id + extension;
                var absImagePath = Path.Combine(wwrootPath,relativeImagePath);
                using (var fileStream = new FileStream(absImagePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                savedProduct.Image = relativeImagePath;
                _context.SaveChanges();
            }

           return Redirect("/Index");
        }
        
        return Page();
    }

}