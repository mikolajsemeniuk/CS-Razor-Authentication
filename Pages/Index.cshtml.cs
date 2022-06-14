using Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Data;
using Microsoft.EntityFrameworkCore;
using Enums;
using System.Diagnostics;

namespace application.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DataContext _context;
    private readonly SignInManager<Entities.Account> _signInManager;
    private readonly UserManager<Entities.Account> _userManager;

    public IndexModel(ILogger<IndexModel> logger, DataContext context, UserManager<Entities.Account> userManager, SignInManager<Entities.Account> signIn)
    {
        _logger = logger;
        _context = context;
        _signInManager = signIn;
        _userManager = userManager;
    }
    
    public Product Product { get; set; } = new();
    public IEnumerable<Product> Products {get;set; }
    public IEnumerable<Category> Categories = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
    public bool chekPara = false;
    public string Category { get; set; }
    public async Task<IActionResult> OnGet([FromQuery(Name = "id")] string? id)
    {
        Products = await _context.Products.ToListAsync();
        if(id!=null){
            Products = Products.Where(p=>p.Category.ToString() == id);
            Category = id;
            chekPara =true;
        }
        return Page();
    }
    
}
