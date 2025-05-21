using DataAccessLayer;
using DataAccessLayer.Models;
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

        public List<FeaturedProduct> FeaturedProducts { get; set; } = new();

        public async Task OnGetAsync()
        {
            FeaturedProducts = await _context.FeaturedProducts
                .OrderBy(fp => fp.Name)
                .ToListAsync();
        }
    }
}
