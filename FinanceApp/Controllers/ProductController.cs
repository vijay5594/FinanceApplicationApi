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
    public class ProductController : ControllerBase
    {
        private readonly UserDbContext context;

        public ProductController(UserDbContext userdbcontext)
        {
            context = userdbcontext;
        }



        [HttpPost("AddNewProduct")]
        public IActionResult AddProductDetails([FromBody] ProductModel productObj)
        {



            if (!context.ProductModels.Any(a => a.ProductName == productObj.ProductName))
                {
                
                context.ProductModels.Add(productObj);
                context.SaveChanges();
                return Ok(productObj);
            }
            else
            {
                return BadRequest();
            }
    
        }
        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProductDetails([FromBody] ProductModel productObj)
        {

            var product = context.ProductModels.AsNoTracking().FirstOrDefault(a => a.ProductId == productObj.ProductId);

            if (product !=null)
            {
                try
                {
                    context.ProductModels.Attach(productObj);
                    context.Entry(productObj).State = EntityState.Modified;
                    context.SaveChanges();
                    return Ok(productObj);
                }
                catch
                {

                }
            }
            return BadRequest();

        }
        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProductDetails(int ProductId)
        {
            var products = context.ProductModels.Where(a => a.ProductId == ProductId).FirstOrDefault();

           
            if (products == null)
            {
                return BadRequest();

            }
            else
            {
                products.IsActive = false;
                context.SaveChanges();
                return Ok();
            }

        }
        [HttpGet("Allproducts")]
        public IActionResult GetAllProducts()
        {
            bool IsActive = true;
            var products = context.ProductModels.Where(a => a.IsActive == IsActive);
            return Ok(products);
        }
    }
}

