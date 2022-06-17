using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Data;
using FinanceApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers
{
    [Route("api/[controller]")]
    public class ProductCustomerController : Controller
    {
        private readonly UserDbContext context;

        public ProductCustomerController(UserDbContext userdbcontext)
        {
            context = userdbcontext;
        }
        [HttpPost("AddProductCustomerdetails")]
        public IActionResult AddProductCustometDetails([FromBody] ProductCustomerModel userObj)

        {
            try
            {
                var slotno = (from a in context.ProductCustomerModels where a.ProductId == userObj.ProductId select a.SlotNo).Max();
                userObj.SlotNo = slotno + 1;
                if (context.ProductModels.Any(a => a.ProductId == userObj.ProductId && a.NumberOfCustomers >= userObj.SlotNo) &&
            context.CustomerModels.Any(a => a.CustomerId == userObj.CustomerId) &&
           !context.ProductCustomerModels.Any(a => a.ProductId == userObj.ProductId && a.SlotNo == userObj.SlotNo))
                {

                    context.ProductCustomerModels.Add(userObj);
                    context.SaveChanges();
                    return Ok(userObj);

                }
            }
            catch (InvalidOperationException)
            {
                userObj.SlotNo = 1;
                context.ProductCustomerModels.Add(userObj);
                context.SaveChanges();
                return Ok(userObj); ;
            }
            return BadRequest();

        }



        [HttpPut("UpdateProductCustomer")]
        public IActionResult UpdateProductCustomerDetails([FromBody] ProductCustomerModel userObj)
        {

            var productcustomer = context.ProductCustomerModels.AsNoTracking().FirstOrDefault(a => a.ProductCustomerId == userObj.ProductCustomerId);
            if (productcustomer != null && userObj.IsActive == true)
            {
                context.Entry(userObj).State = EntityState.Modified;
                context.SaveChanges();
                return Ok(userObj);

            }
            else if (productcustomer != null && userObj.IsActive == false)
            {
                productcustomer.IsActive = true;
                context.SaveChanges();
                return Ok(userObj);
            }
            else
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Product Not Found"
                });
            }
        }
        [HttpDelete("DeleteProductCustomer")]
        public IActionResult DeleteProductCustomer(int ProductCustomerId)
        {
            var productcustomer = context.ProductCustomerModels.Where(a => a.ProductCustomerId == ProductCustomerId).FirstOrDefault();


            if (productcustomer != null && productcustomer.IsActive == true)
            {
                productcustomer.IsActive = false;
                context.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Product customer Details deleted successfully"
                });

            }
            else
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Product customer Not Found"
                });
            }

        }
        [HttpGet("AllproductCustomer")]
        public IActionResult GetAllProductCustomer()
        {

            bool IsActive = true;
            var products = context.ProductCustomerModels.Where(a => a.IsActive == IsActive);
            return Ok(products);
        }
    
        [HttpGet("OrderByProduct")]
        public IActionResult GetById(int ProductId)
        {
            bool IsActive = true;
            var products = context.ProductCustomerModels.Where(a => a.ProductId == ProductId && a.IsActive == IsActive).ToList();
            
            if (products!=null)
            {

                return Ok(products);

            }
            return NotFound();
        }
    }
}

