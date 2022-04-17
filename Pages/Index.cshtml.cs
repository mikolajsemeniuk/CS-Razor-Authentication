using Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nest;

namespace application.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IElasticClient _elasticClient;

    public IndexModel(ILogger<IndexModel> logger, IElasticClient elasticClient)
    {
        _logger = logger;
        _elasticClient = elasticClient;
    }

    // IEnumerable<Product>?
    public async Task OnGet()
    {
        var response = await _elasticClient.SearchAsync<Product>(s => s.Index("products"));
        var documents = response?.Documents?.ToList();
        // return documents;
    }
}
