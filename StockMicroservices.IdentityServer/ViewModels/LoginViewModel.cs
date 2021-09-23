using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace StockMicroservices.IdentityServer.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public IEnumerable<AuthenticationScheme> ExternalProviders { get; set; }
    }
}
