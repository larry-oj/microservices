using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KnowYourPostView.Pages;

public class RegisterModel : PageModel
{
    private readonly ILogger<RegisterModel> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;

    public RegisterModel(ILogger<RegisterModel> logger, 
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
        var response = await client.PostAsJsonAsync($"{service1Url}/user", new
        {
            email = Email,
            password = Password
        });

        if (response.IsSuccessStatusCode)
        {
            return RedirectToPage("/Account/Login");
        }

        return Page();
    }
}