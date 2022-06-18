using Data;
using Entities;
using Helpers;
using Inputs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
// using Microsoft.Extensions.Hosting.Internal;

namespace application.Pages.ProductPage;
public class Cart : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DataContext _context;
    private readonly SignInManager<Entities.Account> _signInManager;
    
    public Cart(ILogger<IndexModel> logger, DataContext context, SignInManager<Entities.Account> signInManager)
    { 
        _logger = logger;
        _context = context;
        _signInManager = signInManager;
    }

    public decimal Total = 0;
    public List<Item> cart;

    public  IActionResult OnGet([FromQuery(Name = "id")] string? id)
    {
        
        if(id != null){       
        var product = _context.Products.Where(p => p.Id == Guid.Parse(id)).FirstOrDefault();

            cart = SessionHelper.GetObjecFromJson<List<Item>>(HttpContext.Session, "cart");

            if(cart == null)
            {
                cart = new List<Item>();
                cart.Add(new Item(){
                    Product = product,
                    Quantity = 1
                });
                SessionHelper.SerObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                var index = Exists(cart, id);
                if(index == -1)
                {
                    cart.Add(new Item(){
                    Product = product,
                    Quantity = 1
                });
                }
                else
                {
                    var newQuantity = cart[index].Quantity +1;
                    cart[index].Quantity = newQuantity;
                }
                SessionHelper.SerObjectAsJson(HttpContext.Session, "cart", cart);
            }
         return Page();
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