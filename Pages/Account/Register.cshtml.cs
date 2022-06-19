using Inputs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace application.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly UserManager<Entities.Account> _userManager;
    private readonly SignInManager<Entities.Account> _signInManager;
    
    [BindProperty]
    public Register Input { get; set; } = new();

    public RegisterModel(UserManager<Entities.Account> userManager, SignInManager<Entities.Account> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public IActionResult OnGet()
    {
        if (_signInManager.IsSignedIn(User))
        {
            return RedirectToPage("/Index");
        }
        return Page();
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
            if (exists != null)
            {
                ModelState.AddModelError("UserNameOccupied", "User name already taken try another one");
                return Page();
            }

            exists = await _userManager.FindByEmailAsync(Input.EmailAddress.ToLower());
            if (exists != null)
            {
                ModelState.AddModelError("EmailOccupied", "Email already taken try another one");
                return Page();
            }

            var account = new Entities.Account
            {
                UserName = Input.UserName.ToLower(),
                Email = Input.EmailAddress
            };

            var result = await _userManager.CreateAsync(account, Input.Password);
            if (result.Succeeded)
            {
                // await _userManager.AddToRolesAsync(account, new[] { "Member" });
                await _signInManager.SignInAsync(account, isPersistent: false);
                return LocalRedirect("/Index");
            }

            ModelState.AddModelError("RegisterError", "There was a problem sign you up, try later");
            return Page();
        }

        return Page();
    }
}