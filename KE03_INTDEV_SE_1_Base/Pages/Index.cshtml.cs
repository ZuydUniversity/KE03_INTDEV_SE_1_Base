using DataAccessLayer;
using DataAccessLayer.Models;
using KE03_INTDEV_SE_1_Base.Pages.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KE03_INTDEV_SE_1_Base.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MatrixIncDbContext _context;

        public IndexModel(MatrixIncDbContext context)
        {
            _context = context;
        }

        public List<CartItem> CartItems { get; set; } = new();

        public List<FeaturedProduct> FeaturedProducts { get; set; } = new();

        public List<string> Categories { get; set; } = new();

        public async Task OnGetAsync()
        {
            CartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();
            var totalItems = CartItems.Sum(item => item.Quantity);
            ViewData["CartCount"] = totalItems;

            FeaturedProducts = await _context.FeaturedProducts
                .OrderBy(fp => fp.Name)
                .ToListAsync();

            Categories = await _context.Categories
                .Select(p => p.Name)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();

            ViewData["Categories"] = Categories;
        }
    }
}
