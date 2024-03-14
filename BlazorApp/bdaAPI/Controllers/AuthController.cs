using bdaAPI.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly MyDbContext _context;
    public AuthController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet("callback")]
    public async Task<IActionResult> LineCallback(string code, string state)
    {
        var clientId = "2003996418";
        var clientSecret = "af1ca219bda830894a12795187237083";
        var redirectUri = "https://localhost:7145/Auth/callback";

        using (var httpClient = new HttpClient())
        {
            var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", redirectUri },
                { "client_id", clientId },
                { "client_secret", clientSecret }
            });

            HttpResponseMessage response = await httpClient.PostAsync("https://api.line.me/oauth2/v2.1/token", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var tokenData = JsonConvert.DeserializeObject<dynamic>(content);
                var accessToken = tokenData.access_token.ToString();

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var profileResponse = await httpClient.GetAsync("https://api.line.me/v2/profile");

                if (profileResponse.IsSuccessStatusCode)
                {
                    var profileContent = await profileResponse.Content.ReadAsStringAsync();
                    var profile = JsonConvert.DeserializeObject<Account>(profileContent);

                    // 認証クッキーの生成
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, profile.userId), // これが 'sub' claim として使用される
                        new Claim("sub", profile.userId), // 明示的に 'sub' claim を追加
                        new Claim(ClaimTypes.Name, profile.displayName),
                        // 他に必要なクレームがあればここに追加
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        // 認証クッキーの設定、必要に応じてここで調整
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return Redirect("https://localhost:7165"); // クライアントにリダイレクト
                }
                else
                {
                    return BadRequest("Failed to retrieve LINE profile.");
                }
            }
            else
            {
                return BadRequest("Failed to retrieve LINE access token.");
            }
        }
    }
}
