using System.Globalization;

namespace EventApp.Models;

public record ProductColor(string Name, string Hex);

public class Product
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Category { get; init; }
    public required string Description { get; init; }
    public decimal Price { get; init; }
    public string[] Sizes { get; init; } = [];
    public ProductColor[] Colors { get; init; } = [];
    public string? Badge { get; init; }            // "NEW" or "LIMITED"
    public DateOnly Released { get; init; }
    public int Stock { get; init; }
    public string[] Images { get; init; } = [];    // empty = CSS placeholder
    public Dictionary<string, string> ColorImages { get; init; } = new(); // colorName -> path
    public string PriceText => Money.Peso(Price);
}

public static class Money
{
    public static string Peso(decimal v) => "₱" + v.ToString("N0", CultureInfo.InvariantCulture);
}