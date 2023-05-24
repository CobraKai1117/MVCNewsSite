using System;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using NuGet.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;

[ApiController]
[Route("api/[controller]/{id}")]
public class ExternalLoginAuthController : ControllerBase
{
    
    private readonly IHttpClientFactory _httpClientFactory;
   // private readonly HttpContext sessionToken;

    public ExternalLoginAuthController(IHttpClientFactory httpClientFactory)
    {

       // sessionToken = _httpContext;
        _httpClientFactory = httpClientFactory;
    }




   /* public async Task<GoogleJsonWebSignature.Payload> AuthenticateJwtToken(string jwtString)
    {
        try
        {
            // Validate and authenticate the JWT token
            var settings = new GoogleJsonWebSignature.ValidationSettings();
            var payload = await GoogleJsonWebSignature.ValidateAsync(jwtString, settings);

            // JWT token is valid and authenticated
            // Access the payload to retrieve the claims and perform further processing if needed

            // Example: Output the email claim
            Console.WriteLine($"Email: {payload.Email}");

            return payload;
        }
        catch (InvalidJwtException ex)
        {
            // JWT token is invalid or failed to authenticate
            Console.WriteLine($"Invalid JWT token: {ex.Message}");
            return null;
        }
    } */




    [HttpGet("login")]
    public IActionResult ExternalLogin()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("HandleExternalLogin", "ExternalLoginAuth")
        };

        return Challenge(authenticationProperties, "Google");
    }

    // ... your other methods



    [HttpPost]
    public async Task<IActionResult> HandleExternalLogin()
    {



        // Retrieve the token from the form data
        var encodedToken = Request.Form["credential"].ToString();
        var jwtHandler = new JwtSecurityTokenHandler();

        var jwtToken = jwtHandler.ReadToken(encodedToken);

        try
        {

            var settings = new GoogleJsonWebSignature.ValidationSettings();
            var payload = await GoogleJsonWebSignature.ValidateAsync(encodedToken, settings);
            ChallengeResult result = new ChallengeResult("Google");

        }

        catch(InvalidJwtException tokenInvalid) 
        
        {

            Console.WriteLine("Message:" + " " + tokenInvalid.Message);
        
        }


        // sessionToken.Session.SetString("GoogleToken", encodedToken);


        return RedirectToPage("/Account/ExternalLogin");


       // return LocalRedirect("https://localhost:7051/api/ExternalLoginAuth/HandleExternalLogin");


        //JwtSecurityToken test = new JwtSecurityToken();

        // test.Issuer = token.

        // Validate the JWT token
        /*    var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "https://accounts.google.com",
                ValidateAudience = true,
                ValidAudience = "627736457817-r1285kn2vekomu4jb4967delhg3n5dr4.apps.googleusercontent.com",
                //IssuerSigningKeys = await GetSigningKeysAsync()
                // Validation parameters configuration
            };



            var tokenHandler = new JwtSecurityTokenHandler();
           // SecurityToken validatedToken;
           // ClaimsPrincipal claimsPrincipal;


            try
            {

               claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out validatedToken);
            }
            catch (SecurityTokenException)
            {
                return BadRequest("Invalid token");
            }

            // Retrieve user information from the validated token
           // var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
           // var name = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;

            // Perform necessary actions such as creating or logging in the user using the retrieved information
            // ...

            // Return appropriate response based on the processing result */
      //  return LocalRedirect()
            
            //Ok("Login successful");
    }

    private async Task<IEnumerable<SecurityKey>> GetSigningKeysAsync()
    {
        var jwksUrl = "https://www.googleapis.com/oauth2/v3/certs";

        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync(jwksUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to retrieve signing keys from JWKS endpoint. StatusCode: {response.StatusCode}");
        }

        

       

        var jwksJson = await response.Content.ReadAsStringAsync();
        var customJwks = JsonSerializer.Deserialize<JsonWebKeySet>(jwksJson);


        return (IEnumerable<SecurityKey>)customJwks.Keys;
    }
}

/*public class ExternalLoginToken
{
    public string Token { get; set; }
} */



