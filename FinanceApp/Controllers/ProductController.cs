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



            var prod = context.ProductModels.AsNoTracking().FirstOrDefault(z => z.ProductName == productObj.ProductName);
            if(prod == null)
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

            if (product == null)
            {
                return BadRequest();
            }
            else
            {
                context.Entry(productObj).State = EntityState.Modified;
                context.SaveChanges();
                return Ok(productObj);
            }

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

