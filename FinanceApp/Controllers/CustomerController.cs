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
            return BadRequest();
                
            }


        [HttpPut("UpdateCustomer")]
        public IActionResult UpdateCustomerDetails([FromBody] CustomerModel userObj)
        {

             var customer = context.CustomerModels.AsNoTracking().FirstOrDefault(a => a.CustomerId == userObj.CustomerId);
            if (customer != null)

            {
                try
                {
                    context.CustomerModels.Attach(userObj);
                    context.Entry(userObj).State = EntityState.Modified;
                    context.SaveChanges();
                    return Ok(userObj);
                }
                catch
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [HttpDelete("DeleteCustomer")]
        public IActionResult DeleteCustomerDetails(int CustomerId)
        {
            var customer = context.CustomerModels.Where(a => a.CustomerId == CustomerId).SingleOrDefault();
           
            
            if(customer!=null&&customer.IsActive==true)    
            {
                customer.IsActive = false;
                context.SaveChanges();
                return Ok();

            }
          
                return NotFound();
          

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

