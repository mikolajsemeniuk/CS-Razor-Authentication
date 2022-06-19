using Data;
using Entities;
using Encoding;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace application.Pages.ProductPage;
public class Cart : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DataContext _context;
    private readonly SignInManager<Entities.Account> _signInManager; 
    public decimal Total = 0;
    public List<Item> cart = new();

    public Cart(ILogger<IndexModel> logger, DataContext context, SignInManager<Entities.Account> signInManager)
    { 
        _logger = logger;
        _context = context;
        _signInManager = signInManager;
    }

    public IActionResult OnGet([FromQuery(Name = "id")] string? id)
    {
        //Total = cart.Sum(item => item.Product.Price * item.Quantity);
        if(id != null)
        {
            // FIXME: handle not found
            var product = _context.Products.Where(p => p.Id == Guid.Parse(id)).FirstOrDefault();
            if (product == null)
            {
                Total = Total + cart.Sum(item => item.Product.Price * item.Quantity);
                // FIXME: return to not found page
                return Page();
            } 

            cart = JSON.Unmarshal<List<Item>>(HttpContext.Session, "cart") ?? new List<Item>();
            if(cart == null)
            {
                cart = new List<Item>();
                Total = cart.Sum(item => item.Product.Price * item.Quantity);                
                cart.Add(new Item(product, 1));              
                JSON.Marshal(HttpContext.Session, "cart", cart); 
            }
            else
            {
                Total = cart.Sum(item => item.Product.Price * item.Quantity);
                var index = Exists(cart, id);
                if(index == -1)
                {
                    cart.Add(new Item(product, 1));
                    Total =  cart.Sum(item => item.Product.Price * item.Quantity);
                }
                else
                {
                    var newQuantity = cart[index].Quantity +1;
                    cart[index].Quantity = newQuantity;
                    Total = Total + cart.Sum(item => item.Product.Price * item.Quantity);
                }
                JSON.Marshal(HttpContext.Session, "cart", cart);
            }
            
        }
        else
        {
            try{}
            catch{
                throw new Exception("Product unknown");
            }      
        }

        int Exists(List<Item> cart, string id)
        {
            for(int i = 0; i < cart.Count; i++)
            {
                if(cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
    return Page();
    }

}