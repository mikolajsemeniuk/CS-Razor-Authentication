using Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Data;
using Microsoft.EntityFrameworkCore;
using Enums;

namespace application.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DataContext _context;
    private readonly SignInManager<Entities.Account> _signInManager;
    private readonly UserManager<Entities.Account> _userManager;
    private readonly IPersonService _personService;
    public int Count { get; set; } = 0;
    public int PageSize { get; set; } = 4;
    public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public IEnumerable<Category> Categories { get; set; } = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
    public bool ifParameterIsSet { get; set; } = false;
    public string CategoryHelper { get; set; } = String.Empty;

    public IndexModel(IPersonService personService, ILogger<IndexModel> logger, DataContext context, UserManager<Entities.Account> userManager, SignInManager<Entities.Account> signIn)
    {
        _personService = personService;
        _logger = logger;
        _context = context;
        _signInManager = signIn;
        _userManager = userManager;

    }

    public async Task<IActionResult> OnGet([FromQuery(Name = "id")] string? category, [FromQuery(Name = "currentpage")] int currentpage)
    {
        CategoryHelper = category;

        if(CategoryHelper != null)
        {
           Products = await _personService.GetPaginatedResultWithFilter(currentpage, PageSize, CategoryHelper);
           ifParameterIsSet =true;
           Count = await _personService.GetCount();
        }
        else{
            Products = await _personService.GetPaginatedResult(currentpage, PageSize);
            Count = await _personService.GetCount();
        }
        
        return Page();
    }

}

// FIXME: move to interfaces
public interface IPersonService
{
    Task<List<Product>> GetPaginatedResult(int currentPage, int pageSize = 10);
    Task<List<Product>> GetPaginatedResultWithFilter(int currentPage, int pageSize = 10, string filter = "Other");
    Task<int> GetCount();
}

// FIXME: move to services folder
public class PersonService : IPersonService
{
    private readonly DataContext _context;

    public PersonService(DataContext context)
    {
        _context = context;
    }

    public int Count { get; set; }
    
    public async Task<int> GetCount()
    {
        return Count;
    }

    public async Task<List<Product>> GetPaginatedResult(int currentPage, int pageSize = 10)
    {
        var data = await GetData();
        Count = data.Count;
        return data.OrderBy(d => d.Id).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
    }

    public async Task<List<Product>> GetPaginatedResultWithFilter(int currentPage, int pageSize = 10, string filter = "Other")
    {
        if(filter == "Category")
        {
        var data = await GetData();
        Count = data.Count;
        return data.OrderBy(d => d.Id).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
        else
        {
        Enums.Category category = (Category)Enum.Parse(typeof(Category),filter);
        var data = await GetData();
        data = data.Where(p=>p.Category == category).ToList();
        Count = data.Count;
        return data.OrderBy(d => d.Id).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

    }

    private async Task<List<Product>> GetData()
    {
       return  await _context.Products.ToListAsync();
    }
}