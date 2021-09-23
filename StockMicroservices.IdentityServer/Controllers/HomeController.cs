using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockMicroservices.IdentityServer.Models;
using StockMicroservices.IdentityServer.ViewModels;

namespace StockMicroservices.IdentityServer.Controllers
{
    public class HomeController : Controller
    {
        #region Fields
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;
        private readonly ILogger<HomeController> _logger;
        #endregion

        public HomeController(
            ILogger<HomeController> logger,
            SignInManager<ApplicationUser> signManager,
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService identityServerInteractionService)
        {
            _logger = logger;
            _signInManager = signManager;
            _userManager = userManager;
            _identityServerInteractionService = identityServerInteractionService;
        }

        #region Methods

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var externalProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();
            if (externalProviders == null)
            {
                externalProviders = new List<AuthenticationScheme>();
            }
            var loginViewModel = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                ExternalProviders = externalProviders
            };
            return View(loginViewModel);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);

            if (result.Succeeded)
            {
                if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Redirect(loginViewModel.ReturnUrl);
                }
            }

            return View();
        }

        public IActionResult Register(string returnUrl)
        {
            var registerViewModel = new RegisterViewModel()
            {
                ReturnUrl = returnUrl
            };

            return View(registerViewModel);
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            ApplicationUser user = new ApplicationUser()
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                if (string.IsNullOrEmpty(registerViewModel.ReturnUrl))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Redirect(registerViewModel.ReturnUrl);
                }
            }

            return View();
        }

        /// <summary>
        /// Log out
        /// </summary>
        /// <param name="logoutId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _identityServerInteractionService.GetLogoutContextAsync(logoutId);

            if (logoutRequest == null || string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

        [HttpPost]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            //var redirectUri = Url.Action(nameof(ExternalLoginCallback), "Auth", new { returnUrl });
            //var externalAuthProperties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);
            //return Challenge(externalAuthProperties, provider);
            return null;
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            //Nothing happened
            if (info == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            ///To be continued
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }

            return null;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
