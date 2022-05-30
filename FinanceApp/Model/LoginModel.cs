using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Model
{
    public class LoginModel
    {
        [Key]
       
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
