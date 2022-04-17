using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace application.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly SignInManager<Entities.Account> _signInManager;

    public LogoutModel(SignInManager<Entities.Account> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnGet()
    {
        await _signInManager.SignOutAsync();
        return RedirectToPage("/Index");
    }
}