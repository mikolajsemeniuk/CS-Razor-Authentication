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
    //public int CurrentPage { get; set; } = 1;
    public int Count { get; set; }
    public int PageSize { get; set; } = 4;
    public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
    public IndexModel(IPersonService personService, ILogger<IndexModel> logger, DataContext context, UserManager<Entities.Account> userManager, SignInManager<Entities.Account> signIn)
    {
        _personService = personService;
        _logger = logger;
        _context = context;
        _signInManager = signIn;
        _userManager = userManager;
    }

    public IEnumerable<Product> Products {get;set; } 
    public IEnumerable<Category> Categories = Enum.GetValues(typeof(Category)).Cast<Category>().ToList();
    public bool ifParameterIsSet { get; set; }= false;
    public string Category { get; set; }
    public async Task<IActionResult> OnGet([FromQuery(Name = "id")] string? id, [FromQuery(Name = "currentpage")] int currentpage)
    {
        //Products = await _context.Products.ToListAsync();

    if(currentpage != null){
                Products = await _personService.GetPaginatedResult(currentpage, PageSize);
    }
        //Products = await _personService.GetPaginatedResult(CurrentPage, PageSize);
        Count = await _personService.GetCount();
        if(id!=null && id !="Category"){
            Products = Products.Where(p=>p.Category.ToString() == id);
            Category = id;
            ifParameterIsSet =true;
            return Page();
        }
        return Page();
    }
}
    public interface IPersonService
    {
    Task<List<Product>> GetPaginatedResult(int currentPage, int pageSize = 10);
    Task<int> GetCount();
    }

public class PersonService : IPersonService
{
    private readonly DataContext _context;
    public PersonService(DataContext context)
    {
        _context = context;
    }
    public async Task<int> GetCount()
    {
        var data = await GetData();
        return data.Count;
    }

    public async Task<List<Product>> GetPaginatedResult(int currentPage, int pageSize = 10)
    {
        var data = await GetData();
        return data.OrderBy(d => d.Id).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
    }
    private async Task<List<Product>> GetData()
    {
       return  await _context.Products.ToListAsync();
    }
}