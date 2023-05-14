using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

[ApiController]
[Route("api/[controller]/{id}")]
public class ExternalLoginAuthController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ExternalLoginAuthController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }


    [HttpPost]
    public async Task<IActionResult> HandleExternalLogin([FromBody] ExternalLoginToken token)
    {
        // Validate the JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "google", // Replace with the actual issuer value
            ValidateAudience = true,
            ValidAudience = "627736457817-r1285kn2vekomu4jb4967delhg3n5dr4.apps.googleusercontent.com", // Replace with the actual audience value
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = await GetSigningKeysAsync() // Replace with the actual signing key retrieval logic
        };

        ClaimsPrincipal claimsPrincipal;

        try
        {
            claimsPrincipal = tokenHandler.ValidateToken(token.Token, validationParameters, out _);
        }

        catch (SecurityTokenException)
        {
            return BadRequest("Invalid token");
        }

        // Retrieve user information from the validated token
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        var name = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;

        // Perform necessary actions such as creating or logging in the user using the retrieved information
        // ...

        // Return appropriate response based on the processing result
        return Ok("Login successful");
    }

    private async Task<IEnumerable<SecurityKey>> GetSigningKeysAsync()
    {
        var jwksUrl = "https://www.googleapis.com/oauth2/v3/certs"; // JWKS endpoint URL

        var response = await _httpClient.GetAsync(jwksUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to retrieve signing keys from JWKS endpoint. StatusCode: {response.StatusCode}");
        }

        var jwksJson = await response.Content.ReadAsStringAsync();

        var jwks = JsonSerializer.Deserialize<JsonWebKeySet>(jwksJson);

        return jwks.Keys;
    }
}

public class ExternalLoginToken
{
    public string Token { get; set; }
}
