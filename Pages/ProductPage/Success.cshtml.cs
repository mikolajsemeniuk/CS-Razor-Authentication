
using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace application.Pages.ProductPage;

public class SuccessModel : PageModel
{
    public SuccessModel()
    {
        
    }

    public IActionResult OnGet()
    {
        return Page();
    }
}