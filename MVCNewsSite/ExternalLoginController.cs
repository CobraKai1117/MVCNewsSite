using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVCNewsSite
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalLoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult VerifyGoogleToken([FromBody] TokenVerificationRequest request)
        {
            // Validate the JWT token received from Google
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = "YOUR_ISSUER", // Replace with the appropriate issuer value
                ValidateAudience = true,
                ValidAudience = "YOUR_AUDIENCE", // Replace with the appropriate audience value
                ValidateLifetime = true,
               // IssuerSigningKey = "YOUR_SIGNING_KEY" // Replace with the appropriate signing key
            };

            try
            {
                // Validate the token and extract claims
                var claimsPrincipal = tokenHandler.ValidateToken(request.IdToken, validationParameters, out _);
                var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

                // Perform additional authentication or generate a session token

                // Return the result to the caller
                return Ok(new TokenVerificationResponse { Success = true, Email = email });
            }
            catch (SecurityTokenValidationException ex)
            {
                // Token validation failed
                // Handle the error as per your application's requirements
                return BadRequest(new TokenVerificationResponse { Success = false, ErrorMessage = ex.Message });
            }
        }
    }

    public class TokenVerificationRequest
    {
        public string IdToken { get; set; }
    }

    public class TokenVerificationResponse
    {
        public bool Success { get; set; }
        public string Email { get; set; }
        public string ErrorMessage { get; set; }
    }
}
