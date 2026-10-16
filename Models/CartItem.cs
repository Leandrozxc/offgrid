namespace EventApp.Models;

public class CartItem
{
    public required Product Product { get; init; }
    public required string Size { get; init; }
    public required string Color { get; init; }
    public int Quantity { get; set; }
    public decimal LineTotal => Product.Price * Quantity;
    public string Key => $"{Product.Id}|{Size}|{Color}";
}