using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KnowYourPostView.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;

    public IndexModel(ILogger<IndexModel> logger, 
        IHttpClientFactory clientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _clientFactory = clientFactory;
        _configuration = configuration;
    }

    public SelectList Countries { get; set; }

    [BindProperty]
    public string CountryProp { get; set; } = "";

    [BindProperty]
    public double WeightProp { get; set; }

    [BindProperty]
    public double PriceProp { get; set; }

    [BindProperty(SupportsGet = true)]
    public double ResultProp { get; set; }

    public async Task OnGetAsync()
    {
        using var client = _clientFactory.CreateClient();
        var service2url = _configuration["service2url"];
        var response = await client.GetAsync($"{service2url}/countries");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Error calling service 2");

        var content = await response.Content.ReadAsStringAsync();
        Countries = new(JsonSerializer.Deserialize<List<string>>(content));
    }

    public async Task<IActionResult> OnPostAsync() 
    {
        await OnGetAsync();

        using var client = _clientFactory.CreateClient();
        var service2Url = _configuration["service2url"];
        var response = await client.PostAsJsonAsync($"{service2Url}/tax/calculate", 
            new { country = CountryProp, price = PriceProp, weight = WeightProp });

        if (!response.IsSuccessStatusCode)
            throw new Exception("Error calling service 2");

        var content = await response.Content.ReadAsStringAsync();
        ResultProp = JsonSerializer.Deserialize<Result>(content)!.Price - PriceProp;

        return Page();
    }
}


public class Result
{
    [JsonPropertyName("price")] public double Price { get; set; }
}