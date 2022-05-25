using Data;
using Entities;
using Inputs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
// using Microsoft.Extensions.Hosting.Internal;

namespace application.Pages.ProductPage;
public class AnnounceModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DataContext _context;
    private readonly IWebHostEnvironment _env;
    
    // private readonly HostingEnvironment _hostingEnvironment;
    
    [BindProperty]
    public Product Product { get; set; } = new();

    public AnnounceModel(ILogger<IndexModel> logger, DataContext context, IWebHostEnvironment env)
    {
        _logger = logger;
        _context = context;
        _env = env;
        //_hostingEnvironment = hostingEnvironment;
    }

[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPost([Bind("Name,Price,Size,Color,Description,Image,Created")] Product product )//,IFormFile file
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
            if (files.Count !=0)
            {
//                 string folderName = "Uploads/Profile/" + UserDb.ID;    //"Uploads/Profile/2017"
//                                    //change here...
//                 string webRootPath = _hostEnvironment.WebRootPath;    //"C:\\YourSolutionLocation\\wwwroot"   
//                 string newPath = Path.Combine(webRootPath, folderName);//"C:\\YourSolutionLocation\\wwwroot\\Uploads/Profile/2017"
// //....
//                 string envpath = folderName + "/" + fileName;  

                

                string imagePath = Path.Combine(_env.WebRootPath, "Images");
               // var imagePath =@"\ProductPage\Announce\";
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
            // string uniqueFileName = null;  //to contain the filename
            //   //handle iformfile
            // {
            //     string uploadsFolder = Path.Combine(_env.WebRootPath, "Images");
            //     uniqueFileName =file.FileName;
            //     string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //     using (var fileStream = new FileStream(filePath, FileMode.Create))
            //     {
            //         file.CopyTo(fileStream);
            //     }
            // }
            // product.Image = uniqueFileName; //fill the image property
              //save
           
           //return RedirectToAction(nameof(Index));
           return Redirect("/Index");
        }
        return Page();
    }

}