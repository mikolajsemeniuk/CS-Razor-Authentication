using System.ComponentModel.DataAnnotations;

namespace Inputs;

public class Login
{
    [Display(Name = "User Name")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Whitespaces are not allowed")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "{0} has to be {2} characters minimum and {1} characters maximum")]
    public string UserName { get; set; } = String.Empty;

    [Display(Name = "Password")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Whitespaces are not allowed")]
    [StringLength(64, MinimumLength = 6, ErrorMessage = "{0} has to be {2} characters minimum and {1} characters maximum")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = String.Empty;
}