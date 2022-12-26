using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KnowYourPostView.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;

    public LoginModel(ILogger<LoginModel> logger, 
        IHttpClientFactory clientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _clientFactory = clientFactory;
        _configuration = configuration;
    }

    [BindProperty]
    public string Email { get; set; } = "";

    [BindProperty]
    public string Password { get; set; } = "";

    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var service1Url = _configuration["service1url"];

        using var client = _clientFactory.CreateClient();
        var response = await client.PostAsJsonAsync($"{service1Url}/user/exists", new
        {
            email = Email,
            password = Password
        });

        if (!response.IsSuccessStatusCode)
        {
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, Email),
            new(ClaimTypes.Email, Email)
        };
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            IsPersistent = true,
            IssuedUtc = DateTimeOffset.UtcNow
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToPage("/Index");
    }
}