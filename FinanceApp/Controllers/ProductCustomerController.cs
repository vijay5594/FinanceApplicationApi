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
            for (userObj.SlotNo = 1; context.ProductModels.Any(a => a.NumberOfCustomers >= userObj.SlotNo); userObj.SlotNo++)
            {
                if (context.ProductModels.Any(a => a.ProductId == userObj.ProductId) &&
                    context.CustomerModels.Any(a => a.CustomerId == userObj.CustomerId) &&
                   !context.ProductCustomerModels.Any(a => a.ProductId == userObj.ProductId && a.SlotNo == userObj.SlotNo))

                {
                    context.ProductCustomerModels.Add(userObj);
                    context.SaveChanges();
                    return Ok(userObj);

                }
                else
                {
                    return NotFound();

                }
            }
            return Ok(userObj);
            
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
            var productcustomer = context.ProductCustomerModels.Where(a => a.ProductCustomerId == ProductCustomerId).SingleOrDefault();


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

            if (IsActive == true)
            {
                var productcustomer = context.ProductCustomerModels.Where(a => a.IsActive == IsActive);

                return Ok(productcustomer);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("OrderByProduct")]
        public IActionResult GetById(int ProductId)
        {
            bool IsActive = true;
            var products = context.ProductCustomerModels.Where(a => a.ProductId == ProductId && a.IsActive == IsActive);
            
            if (products!=null)
            {

                return Ok(products);

            }
            return NotFound();
        }
    }
}

