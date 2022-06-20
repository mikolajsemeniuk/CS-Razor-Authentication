
using Data;
using Encoding;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayPal.Api;

namespace application.Pages.ProductPage;

public class SuccessModel : PageModel
{
    APIContext apicontext = PaypalConfiguration.GetAPIContext();
    private Payment payment;
    public string PaymentId {get; set;}
    public SuccessModel()
    {      
    }

        public IActionResult OnGet([FromQuery(Name = "paymentId")]string paymentId, [FromQuery(Name = "PayerID")] string? PayerId)
        {
            PaymentId = paymentId;

            var executedPaymnt = ExecutePayment(apicontext, PayerId, PaymentId );
            return Page();
        }

        // [HttpPost]
        // [Route("checkout")]
        // public IActionResult OnPost([FromQuery(Name = "paymentId")]string PaymentId)
        // {
        //     var x = 5;
        //     return Page();
        // }


        private object ExecutePayment(APIContext apicontext, string payerId, string PaymentId)
        {
           var  paymentExecution= new PaymentExecution() {payer_id = payerId };
            this.payment= new Payment() {id = PaymentId };
            return this.payment.Execute(apicontext, paymentExecution);
        }
}