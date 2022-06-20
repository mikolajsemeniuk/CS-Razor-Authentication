using application.PayPal;
using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayPal.Api;
namespace application.Pages.ProductPage;

public class PayModel : PageModel
{
    APIContext aPIContext = PaypalConfiguration.GetAPIContext();
    int x = 5;
}