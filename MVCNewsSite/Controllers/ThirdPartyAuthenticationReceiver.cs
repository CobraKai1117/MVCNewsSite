using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace MVCNewsSite.Controllers
{
    public class ThirdPartyAuthenticationReceiver : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;



        public  async Task<IActionResult> GoogleExternalLogin()
        {
            UserManager<IdentityUser> userManager;

          //  userManager.CreateAsync()
            // Retrieve the token from the form data


            var encodedToken = Request.Form["credential"].ToString();
            var jwtHandler = new JwtSecurityTokenHandler();

           JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(encodedToken);

            //var email = jwtToken.Payload.ElementAt(4).Value;
    

           // var user = new IdentityUser { UserName = jwtToken.payload}

            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings();
                var payload = await GoogleJsonWebSignature.ValidateAsync(encodedToken, settings);
            }

            catch(InvalidJwtException invalidGoogleToken) 
            {

                Console.WriteLine("Error message:" + " " + invalidGoogleToken.Message);
            }

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value; // Retrieves user ID from JWT Token
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value; // Retrieves Email from JWT Token



            



            //  return LocalRedirect("/Identity/Account/Login/ExternalLogin");

            return RedirectToPage("/Account/ExternalLogin");
        }
    }
}
