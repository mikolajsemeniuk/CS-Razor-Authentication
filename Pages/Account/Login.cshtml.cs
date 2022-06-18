using Inputs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace application.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<Entities.Account> _signInManager;
    private readonly UserManager<Entities.Account> _userManager;

    [BindProperty]
    public Login Input { get; set; } = new();
    
    public LoginModel(SignInManager<Entities.Account> signInManager, UserManager<Entities.Account> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    public IActionResult OnGet()
    {
        if (_signInManager.IsSignedIn(User))
        {
            return RedirectToPage("/Index");
        }
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var exists = await _userManager.FindByNameAsync(Input.UserName.ToLower());
            if (exists == null)
            {
                ModelState.AddModelError("UserNotExistOccupied", "User with this user name does not exist");
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(Input.UserName.ToLower(), Input.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }

            ModelState.AddModelError("LoginError", "There was a problem sign you in, try later");
            return Page();
        }

        return Page();
    }
}