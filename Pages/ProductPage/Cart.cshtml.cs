using Data;
using Entities;
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
    public Cart(ILogger<IndexModel> logger, DataContext context)
    { 
        _logger = logger;
        _context = context;
    }


    
}