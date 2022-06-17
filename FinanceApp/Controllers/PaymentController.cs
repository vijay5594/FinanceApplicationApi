using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Data;
using FinanceApp.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace FinanceApp.Controllers

{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly UserDbContext context;

        public PaymentController(UserDbContext userdbcontext)
        {
            context = userdbcontext;
        }
        [HttpPost("PaymentDetails")]
        public IActionResult AddPaymentDetails([FromBody] PaymentModel paymentObj)
        {

            if (paymentObj != null && context.ProductCustomerModels.Any(a => a.ProductCustomerId == paymentObj.ProductCustomerId))
            {
                context.PaymentModels.Add(paymentObj);
                context.SaveChanges();


                return Ok(paymentObj);
            }
             return BadRequest();
        }

    }
    
}
