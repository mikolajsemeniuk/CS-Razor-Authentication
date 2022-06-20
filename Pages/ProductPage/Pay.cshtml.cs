using Data;
using Encoding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayPal.Api;
using System.Web;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace application.Pages.ProductPage;

public class PayModel : PageModel
{
    APIContext apicontext = PaypalConfiguration.GetAPIContext();
    public List<Entities.Item> cart;
    private Payment payment;
    private string _payerId {get; set; }
    private readonly SignInManager<Entities.Account> _signInManager;
    private readonly UserManager<Entities.Account> _userManager;
    private readonly ILogger<PayModel> _logger;    

    public PayModel( ILogger<PayModel> logger, DataContext context, UserManager<Entities.Account> userManager, SignInManager<Entities.Account> signIn)
    {
        
        _logger = logger;
        _signInManager = signIn;
        _userManager = userManager;

    }
    public IActionResult OnGet([FromQuery(Name = "id")] string? PayerId)
    {     
        _payerId = PayerId ?? string.Empty;
        apicontext = PaypalConfiguration.GetAPIContext();
        try
        {           
        //var Payer = Request.QueryString;
                // if (string.IsNullOrEmpty(PayerId) && PayerId!=null)
                if (_payerId!=null)
                {
                    //utwórz płatność
                    Guid guid = Guid.NewGuid();
                    string baseURi = Request.Scheme + "://" + Request.Host.Value +
                                     "/ProductPage/Pay?id=" + guid;

                    var createPayment = CreatePayment(apicontext, baseURi, guid);
                    var breakpoint = "break";

                    
                    var links = createPayment.links.GetEnumerator();
                    string paypalRedirectURL = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectURL = lnk.href;
                        }


                    }
                }
                else
                {
                    var guid = Request.Headers["guid"];
                    var Session = JSON.Unmarshal<string>(HttpContext.Session, "guid");
                    
                    var executedPaymnt = ExecutePayment(apicontext, _payerId, Session );

                    if (executedPaymnt.ToString().ToLower()!="approved")
                    {
                         return Page();
                        //return RedirectToPage(""); // błąd płatnosci dodaj strone błedna
                    }

                }
        }
        catch(Exception e)
        {
            return RedirectToPage("/ProductPage/Cancel");
        }

       return RedirectToPage("/ProductPage/Success");
    }
    private object ExecutePayment(APIContext apicontext, string payerId, string PaymentId)
        {
           var  paymentExecution= new PaymentExecution() {payer_id = payerId };
            this.payment= new Payment() {id = PaymentId };
            return this.payment.Execute(apicontext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apicontext, string redirectURl, Guid _invoice_number)
        {

            var ItemLIst = new ItemList() {items = new List<Item>()};

            List<Entities.Item> cart = JSON.Unmarshal<List<Entities.Item>>(HttpContext.Session, "cart") ?? new List<Entities.Item>();
            if (cart!=null)
            {              
                foreach (var item in cart)
                {
                    ItemLIst.items.Add(new PayPal.Api.Item()
                    {
                        name = item.Product.Name.ToString(),
                        description = item.Product.Description.ToString(),
                        quantity = item.Quantity.ToString(),
                        price = "10.00",//item.Product.Price.ToString(),
                        tax = "0.07",
                        sku = "1.00",
                        currency = "USD"                
                    } );
                }

                var payer = new Payer() { payment_method = "paypal" };

                var redirUrl = new RedirectUrls()
                {
                    return_url = "https://localhost:7071/ProductPage/Success",
                    cancel_url = "http://localhost:7071/ProductPage/Cancel"
                };
                string subtotal =cart.Sum(item => item.Product.Price * item.Quantity).ToString().Replace(',','.');
                //string sobtotalReplace  = subtotal1.Replace(',','.');
                var details = new Details()
                {   
                    subtotal = subtotal,
                    tax = "0.07",
                    shipping = "0.03",
                    handling_fee ="1.00",
                    shipping_discount = "-1.00",
                    insurance = "0.01"
                };
                
                //var total1 = decimal.Parse(details.tax.Replace('.',',') + details.shipping.Replace('.',',') + details.subtotal.Replace('.',',') + details.insurance.Replace('.',','));
                //string total = total1.ToString().Replace(',','.');

                var amount = new Amount()
                {
                    total = "10.11", //naprawic zeby nei było zkardkodowane
                    currency = "USD",                    
                    details = details
                };


                var payment_options = new PaymentOptions(){
                    allowed_payment_method = "INSTANT_FUNDING_SOURCE"
                };

                var transactionList = new List<Transaction>();
                transactionList.Add(new Transaction()
                {
                    amount = amount,
                    description = "Transaction Description",
                    custom = "EBAY_EMS_90048630024435",
                    invoice_number = _invoice_number.ToString(),
                    payment_options = payment_options,
                    soft_descriptor = "ECHI5786786",
                    item_list = ItemLIst                    
                });

                var shipping_address = new ShippingAddress()
                {
                    recipient_name= "",
                    line1 = "",
                    line2 ="",
                    city ="",
                    country_code="",
                    postal_code="",
                    phone ="",
                    state=""
                };

                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrl
                };
            }

            return this.payment.Create(apicontext);

        }
}