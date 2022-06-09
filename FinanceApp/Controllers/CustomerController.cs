using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApp.Data;
using FinanceApp.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase

    {
        private readonly UserDbContext context;

        public CustomerController(UserDbContext userdbcontext)


        {
            context = userdbcontext;
        }
        [HttpPost("AddNewCustomer")]
        public IActionResult AddCustomerDetails([FromBody] CustomerModel userObj)
        {


            if (!context.CustomerModels.Any(a => a.MobileNumber == userObj.MobileNumber)
            && !context.CustomerModels.Any(a => a.AadharNumber== userObj.AadharNumber))
            {
                
                context.CustomerModels.Add(userObj);
                context.SaveChanges();
                return Ok(userObj);


            }
            
            
         else
                {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Invalid Customer details cannot be added"
                }); 
                }
            }
                    
        
        [HttpPut("UpdateCustomer")]
        public IActionResult UpdateCustomerDetails([FromBody] CustomerModel userObj)
        {
            
            var customer = context.CustomerModels.AsNoTracking().FirstOrDefault(a => a.CustomerId == userObj.CustomerId);
            if (customer != null && customer.IsActive == true)
            {
                
                    context.Entry(userObj).State = EntityState.Modified;
                    context.SaveChanges();
                    return Ok(userObj);
                }
                 else if (customer != null && customer.IsActive == false)
                {
                    customer.IsActive = true;
                    context.SaveChanges();
                    return Ok(userObj);
                }
              
                else { 
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "CustomerDetails not found"
                });
            }
           
        }
        [HttpDelete("DeleteCustomer")]
        public IActionResult DeleteCustomerDetails(int CustomerId)
        {
            var customer = context.CustomerModels.Where(a => a.CustomerId == CustomerId).SingleOrDefault();
           
            
            if(customer!=null&&customer.IsActive==true)    
            {
                customer.IsActive = false;
                context.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "CustomerDetails deleted successfully"
                });

            }
            else
            {
                return NotFound();
            }

        }
        [HttpGet("AllCustomers")]
        public IActionResult GetAllCustomers()
        {
            bool IsActive = true;
           
            if (IsActive == true)
            {

                var customers = context.CustomerModels.Where(a => a.IsActive == IsActive);
                return Ok(customers);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

