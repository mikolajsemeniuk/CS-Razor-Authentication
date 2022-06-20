using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace application.Pages.ProductPage;

public class CancelModel : PageModel
{
    public CancelModel()
    {

    }

    public IActionResult OnGet()
    {
        return Page();
    }
}