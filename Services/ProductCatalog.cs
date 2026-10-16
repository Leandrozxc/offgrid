using EventApp.Models;
using Microsoft.AspNetCore.Hosting;

namespace EventApp.Services;

public class ProductCatalog
{
    public static readonly string[] Categories = ["Tees", "Hoodies", "Headwear", "Statement Pieces"];
    public static readonly string[] SizeOrder = ["S", "M", "L", "XL", "One Size"];

    static readonly ProductColor Black = new("Black", "#111111");
    static readonly ProductColor Cream = new("Cream", "#F5F1E8");
    static readonly ProductColor Concrete = new("Concrete", "#858585");
    static readonly string[] Apparel = ["S", "M", "L", "XL"];
    static readonly string[] OneSize = ["One Size"];
    static readonly string[] Exts = [".jpg", ".jpeg", ".png", ".webp"];

    static readonly string[] FeaturedIds =
        ["core-oversized-tee", "no-signal-hoodie", "static-graphic-tee", "concrete-logo-cap"];

    readonly IWebHostEnvironment _env;

    public IReadOnlyList<Product> All { get; }

    public ProductCatalog(IWebHostEnvironment env)
    {
        _env = env;

        // view: which photo each color uses ("front" or "back"), matching your filenames:
        // {product-id}-{color}-{view}.jpg
        All =
        [
            Make("core-oversized-tee", "OFFGRID Core Oversized Tee", "Tees", 699,
                "The everyday staple. Boxy, oversized cut with a dropped shoulder and a small chest mark.",
                Apparel, [Black, Cream, Concrete], null, new(2026, 3, 1), 40, "front"),

            Make("out-of-frame-tee", "Out of Frame Tee", "Tees", 749,
                "Oversized fit with an off-center back graphic. Built to be seen from behind.",
                Apparel, [Black, Cream], "NEW", new(2026, 9, 20), 25, "back"),

            Make("daylight-boxy-tee", "Daylight Boxy Tee", "Tees", 649,
                "A cropped-length boxy tee in a soft, relaxed weight for long days on campus.",
                Apparel, [Concrete, Cream], null, new(2026, 5, 10), 30, "front"),

            Make("no-signal-hoodie", "No Signal Heavyweight Hoodie", "Hoodies", 1699,
                "Heavyweight oversized hoodie with a deep hood and a minimal embroidered mark.",
                Apparel, [Black, Concrete], "NEW", new(2026, 9, 27), 18, "front"),

            Make("after-hours-hoodie", "After Hours Hoodie", "Hoodies", 1499,
                "Limited run. Relaxed fit, clean front, and a loud back print built to be seen from behind.",
                Apparel, [Black, Cream], "LIMITED", new(2026, 8, 15), 5, "back"),

            Make("frequency-crewneck", "Frequency Crewneck", "Hoodies", 1199,
                "A clean crewneck with a ribbed collar and a signal-wave graphic on the sleeve.",
                Apparel, [Concrete, Cream], null, new(2026, 6, 2), 22, "front"),

            Make("off-duty-sweatshirt", "Off-Duty Sweatshirt", "Hoodies", 1099,
                "Soft, oversized sweatshirt made for the hours when nobody's watching.",
                Apparel, [Black, Cream], null, new(2026, 4, 18), 20, "front"),

            Make("everyday-cap", "OFFGRID Everyday Cap", "Headwear", 449,
                "Unstructured six-panel cap with an adjustable strap and a tonal logo.",
                OneSize, [Black, Cream, Concrete], null, new(2026, 3, 1), 60, "front"),

            Make("concrete-logo-cap", "Concrete Logo Cap", "Headwear", 549,
                "Structured cap in concrete gray with a raised front logo.",
                OneSize, [Concrete, Black], "NEW", new(2026, 10, 4), 35, "front"),

            Make("static-graphic-tee", "Static Graphic Tee", "Statement Pieces", 849,
                "Oversized tee with a distorted, full-front graphic. Limited run.",
                Apparel, [Black, Cream], "LIMITED", new(2026, 9, 5), 12, "front"),

            Make("unlisted-graphic-tee", "Unlisted Graphic Tee", "Statement Pieces", 899,
                "The one that never made the lookbook. Oversized, loud back print, tiny quantities.",
                Apparel, [Black], "LIMITED", new(2026, 7, 22), 3, "back"),
        ];
    }

    Product Make(string id, string name, string category, decimal price, string description,
                 string[] sizes, ProductColor[] colors, string? badge, DateOnly released, int stock, string view)
    {
        var found = colors
            .Select(c => (c.Name, Path: Find(id, c.Name.ToLowerInvariant(), view)))
            .Where(x => x.Path is not null)
            .ToList();

        return new Product
        {
            Id = id, Name = name, Category = category, Price = price, Description = description,
            Sizes = sizes, Colors = colors, Badge = badge, Released = released, Stock = stock,
            Images = found.Take(1).Select(x => x.Path!).ToArray(),
            ColorImages = found.ToDictionary(x => x.Name, x => x.Path!)
        };
    }

    // Returns "/images/products/{id}-{color}-{view}.ext" if the file exists, otherwise null (placeholder).
    string? Find(string id, string color, string view)
    {
        foreach (var ext in Exts)
        {
            var rel = $"images/products/{id}-{color}-{view}{ext}";
            if (_env.WebRootFileProvider.GetFileInfo(rel).Exists) return "/" + rel;
        }
        return null;
    }

    public static string CategoryUrl(string category) => $"/shop?category={Uri.EscapeDataString(category)}";

    public Product? GetById(string id) =>
        All.FirstOrDefault(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Product> Featured() => FeaturedIds.Select(GetById).OfType<Product>();

    public IEnumerable<Product> NewArrivals(int count) => All.OrderByDescending(p => p.Released).Take(count);

    public IEnumerable<Product> Related(Product product, int count) =>
        All.Where(p => p.Id != product.Id)
           .OrderBy(p => p.Category == product.Category ? 0 : 1)
           .Take(count);

    public IEnumerable<ProductColor> AllColors() => All.SelectMany(p => p.Colors).DistinctBy(c => c.Name);
}