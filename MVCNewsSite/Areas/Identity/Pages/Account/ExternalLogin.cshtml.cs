// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using System.IO;
using System.Net.Http;

namespace MVCNewsSite.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken(Order =1001)]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /* [Required]
             [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
             [DataType(DataType.Password)]
             [Display(Name = "Password")]
             public string Password { get; set; }


             [DataType(DataType.Password)]
             [Display(Name = "Confirm password")]
             [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
             public string ConfirmPassword { get; set; } */

        }

       // public IActionResult OnGet() => RedirectToPage("./Login");

        public async Task OnGetAsync()
        {
            var accessToken = await HttpContext.GetTokenAsync(GoogleDefaults.AuthenticationScheme, "access-token");

        }

        public IActionResult OnPost(string provider = "Google", string returnUrl = null, string token = null) // This is for Registering with an external provider such as Google
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

           
            //redirectUrl);
            return new ChallengeResult(provider, properties); // DON'T NEED THIS METHOD IN EXTERNAL AUTHENTICATION
        }



        public async Task<IActionResult> OnPostCallbackAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // Read the request body to get the token
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var token = await reader.ReadToEndAsync();

                return await ProcessToken(token, returnUrl);
            }
        }

        private async Task<IActionResult> ProcessToken(string token, string returnUrl)
        {
            var payload = await VerifyToken(token); // verify token

            if (payload == null)
            {
                ErrorMessage = "Invalid token";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // find or create the user based on the token
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                // user doesn't exist, create a new one
                user = new IdentityUser { UserName = payload.Email, Email = payload.Email };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    ErrorMessage = "Error creating user";
                    return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                }
            }

            // sign in the user
            var info = await _signInManager.GetExternalLoginInfoAsync();
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (signInResult.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            else
            {
                ErrorMessage = "Error signing in";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
        }

        private async Task<Payload> VerifyToken(string token)
        {
            // verify token using Google's public keys
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={token}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<Payload>(content);
            return payload;
        }

        public class Payload
        {
            [JsonProperty("email")]
            public string Email { get; set; }
        }
    





public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null) 
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information."; 
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.

           // if(info!=null && info.Principal.HasClaim(c => c.Type == ClaimTypes.Email && c.Value == userRegist))
            
            
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }

           // var user = await _userManager.FindByEmailAsync()

          /*  if(User == null) 
            {
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                
                if(info.Principal.HasClaim(c=>c.Type == ClaimTypes.Email)) 
                { 
                    if(info.Principal.FindFirstValue(ClaimTypes.Email) != )

                    {
                        ErrorMessage = "Invalid email.";
                        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                    }

                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }

                return Page();
            
            }

            else 
            {
                var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

                if (signInResult.Succeeded) 
                {
                    _logger.LogInformation("{Name} logged in with {LoginProvider} provider", info.Principal.Identity.Name, info.LoginProvider);
                    return LocalRedirect(returnUrl);
                    
                }

                if(signInResult.IsLockedOut) 
                {
                    return RedirectToPage("./Lockout");
                }

                else 
                {
                    ErrorMessage = "Error signing in with external provider.";
                    return RedirectToPage(".Login", new { ReturnUrl = returnUrl });
                    
                }
            
            } */

            else 
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)  // THIS IS RETRIEVING THE GOOGLE EMAIL EVEN THOUGH I HAVEN'T ENTERED IT
                    };
                }
                return Page();
            } 
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                // If user does not exist, create new user with email and username from Input.Email
                if (user == null)
                {
                   user = new IdentityUser { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true };
                        //PasswordHash = Input.Password, EmailConfirmed = true };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        } 


                        return Page();
                    }
                }

                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                if (addLoginResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    foreach (var error in addLoginResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
