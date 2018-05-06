using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternationalOnlineShopping.Models
{
    public class LoginViewModel
    {
       
            [Required(ErrorMessage = "Email address is required")]
            [DataType(DataType.EmailAddress)]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            
            public string UserEmailId { get; set; }

            [Required(ErrorMessage = "Password is required")]
           
            public string Password { get; set; }

            public int UserType { get; set; }
        
    }
}