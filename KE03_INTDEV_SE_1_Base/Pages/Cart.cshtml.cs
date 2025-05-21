using DataAccessLayer;
using DataAccessLayer.Models;
using KE03_INTDEV_SE_1_Base.Pages.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Webshop.Pages
{
    public class CartModel(MatrixIncDbContext context) : PageModel
    {
        private readonly MatrixIncDbContext _context = context;

        public List<CartItem> CartItems { get; set; } = new();
        public List<string> Categories { get; set; } = new();

        public async Task OnGetAsync()
        {
            CartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            var totalItems = CartItems.Sum(item => item.Quantity);
            ViewData["CartCount"] = totalItems;

            Categories = await _context.Categories
                .Select(p => p.Name)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();

            ViewData["Categories"] = Categories;
        }

        public IActionResult OnPostRemove(int productId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetObject("Cart", cart);
            }

            return RedirectToPage();
        }

        public IActionResult OnPostUpdate(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new List<CartItem>();

            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }

                HttpContext.Session.SetObject("Cart", cart);
            }

            return RedirectToPage();
        }
    }
}
