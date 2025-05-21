using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Webshop.Pages
{
    public class SearchModel(MatrixIncDbContext context) : PageModel
    {
        private readonly MatrixIncDbContext _context = context;

        [BindProperty(SupportsGet = true, Name = "search")]
        public string? Query { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        public List<Product> SearchResults { get; set; } = new();

        public List<string> Categories { get; set; } = new();

        public string PageTitle { get; set; } = "Alle producten";

        public async Task OnGetAsync()
        {
            var queryable = _context.Products
                .Include(p => p.Parts)
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Query))
            {
                var loweredQuery = Query.ToLower();
                queryable = queryable.Where(p =>
                    p.Name.ToLower().Contains(loweredQuery) ||
                    p.Description.ToLower().Contains(loweredQuery));
                PageTitle = $"Alle resultaten voor \"{Query}\"";
            }
            else if (!string.IsNullOrWhiteSpace(Category))
            {
                var loweredCategory = Category.ToLower();
                queryable = queryable.Where(p =>
                    p.Category != null &&
                    p.Category.Name.ToLower().Contains(loweredCategory));
                PageTitle = $"Alle producten in categorie \"{Category}\"";
            }

            Categories = await _context.Categories
                .Select(p => p.Name)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();

            ViewData["Categories"] = Categories;

            SearchResults = await queryable.ToListAsync();
        }
    }
}
