using Microsoft.AspNetCore.Mvc.RazorPages;

namespace application.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;


    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;

    }

    // IEnumerable<Product>?
    public async Task OnGet()
    {

        // return documents;
    }
}
