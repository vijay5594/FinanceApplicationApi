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
    public class LoginController : ControllerBase
    {
        private readonly UserDbContext context;

        public LoginController(UserDbContext userdbcontext)
        {
            context = userdbcontext;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel userObj)
        {

            var user = context.LoginModels.Where(a =>
            a.Username == userObj.Username && a.Password == userObj.Password).FirstOrDefault();
            if (user != null)
            {
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Logged In Successfully"
                });

            }
            else
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "user not found"
                });
            }
        }
        [HttpPost("signup")]
        public IActionResult signup([FromBody] LoginModel userobj)
        {
            if (userobj == null)
            {
                return BadRequest();
            }
            else
            {
                context.LoginModels.Add(userobj);
                context.SaveChanges();

                return Ok(new
                {
                    statuscode = 200,
                    message = "User Added Successfully"
                });
            }
        }
            [HttpPut("ChangePassword")]

        public IActionResult ChangePassword([FromBody] LoginModel userObj)
        {
            var user = context.LoginModels.AsNoTracking().FirstOrDefault(a => a.Username == userObj.Username);
            if (user != null)
            {
                context.Entry(userObj).State = EntityState.Modified;
                context.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Password Changed successfully"
                });
            }
            else
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "user not found"
                });
            }
            
        }
        
    }
}
  



