using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KE03_INTDEV_SE_1_Base.Pages.Helpers;

namespace Webshop.Pages
{
    public class SearchModel(MatrixIncDbContext context) : PageModel
    {
        private readonly MatrixIncDbContext _context = context;

        public List<CartItem> CartItems { get; set; } = new();

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

            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
            ViewData["CartCount"] = cart.Sum(item => item.Quantity);
        }


        public IActionResult OnPostAddToCart(int id, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return RedirectToPage();

            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(i => i.ProductId == id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            HttpContext.Session.SetObject("Cart", cart);

            TempData["ToastMessage"] = $"{quantity}x {product.Name} toegevoegd aan je winkelmandje";

            return RedirectToPage(new { search = Query, category = Category });
        }
    }
}
