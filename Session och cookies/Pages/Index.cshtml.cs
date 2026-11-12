
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Session_och_cookies.Models;
using System.Text.Json;
using Session_och_cookies.Models;

namespace Webshop.Pages;

public class IndexModel : PageModel
{
    public List<Product> Products { get; set; } = new();

    public Cart Cart { get; set; } = new();

    public bool HasConsent { get; set; }

    public string Language { get; set; } = "Svenska";

    public void OnGet()
    {
        LoadProducts();
        LoadCart();

        HasConsent =
            Request.Cookies["CookieConsent"] == "accepted";

        Language =
            Request.Cookies["Language"] ?? "Svenska";
    }

    private void LoadProducts()
    {
        Products = new List<Product>
        {
            new Product
            {
                ProductId = 1,
                Name = "Datormus",
                Price = 199,
                Description = "Trådlös datormus"
            },

            new Product
            {
                ProductId = 2,
                Name = "Tangentbord",
                Price = 499,
                Description = "Mekaniskt tangentbord"
            },

            new Product
            {
                ProductId = 3,
                Name = "Skärm",
                Price = 1999,
                Description = "24-tums datorskärm"
            }
        };
    }

    private void LoadCart()
    {
        string? json =
            HttpContext.Session.GetString("Cart");

        if (json != null)
        {
            Cart = JsonSerializer.Deserialize<Cart>(json)
                ?? new Cart();
        }
        else
        {
            Cart = new Cart();
        }
    }

    private void SaveCart()
    {
        string json =
            JsonSerializer.Serialize(Cart);

        HttpContext.Session.SetString("Cart", json);
    }

    public IActionResult OnPostAdd(int productId)
    {
        LoadProducts();
        LoadCart();

        Product? product =
            Products.FirstOrDefault(p => p.ProductId == productId);

        if (product != null)
        {
            CartItem? existingItem =
                Cart.Items.FirstOrDefault(
                    i => i.Product.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.IncreaseQuantity();
            }
            else
            {
                Cart.Items.Add(new CartItem
                {
                    CartItemId = product.ProductId,
                    Product = product,
                    Quantity = 1
                });
            }
        }

        SaveCart();

        return RedirectToPage();
    }

    public IActionResult OnPostDecrease(int productId)
    {
        LoadCart();

        CartItem? item =
            Cart.Items.FirstOrDefault(
                i => i.Product.ProductId == productId);

        if (item != null)
        {
            item.DecreaseQuantity();

            if (item.Quantity == 0)
            {
                Cart.Items.Remove(item);
            }
        }

        SaveCart();

        return RedirectToPage();
    }

    public IActionResult OnPostClear()
    {
        LoadCart();

        Cart.Clear();

        SaveCart();

        return RedirectToPage();
    }

    public IActionResult OnPostAcceptCookies()
    {
        CookieOptions options = new()
        {
            Expires = DateTimeOffset.UtcNow.AddDays(365),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax
        };

        Response.Cookies.Append(
            "CookieConsent",
            "accepted",
            options);

        return RedirectToPage();
    }

    public IActionResult OnPostRejectCookies()
    {
        TempData["Message"] = "Du har nekat cookies.";

        return RedirectToPage();
    }

    public IActionResult OnPostSetLanguage(string language)
    {
        if (Request.Cookies["CookieConsent"] != "accepted")
        {
            return RedirectToPage();
        }

        if (language != "Svenska" && language != "English")
        {
            return RedirectToPage();
        }

        CookieOptions options = new()
        {
            Expires = DateTimeOffset.UtcNow.AddDays(365),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax
        };

        Response.Cookies.Append(
            "Language",
            language,
            options);

        return RedirectToPage();
    }

    public IActionResult OnPostResetSettings()
    {
        Response.Cookies.Delete("CookieConsent");
        Response.Cookies.Delete("Language");

        TempData["Message"] =
            "Inställningarna har återställts.";

        return RedirectToPage();
    }
}
