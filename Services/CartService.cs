using EventApp.Models;

namespace EventApp.Services;

public class CartService
{
    public const decimal FlatShipping = 150m;
    public const decimal FreeShippingThreshold = 2500m;

    readonly List<CartItem> _items = [];

    public event Action? Changed;

    public IReadOnlyList<CartItem> Items => _items;
    public int Count => _items.Sum(i => i.Quantity);
    public decimal Subtotal => _items.Sum(i => i.LineTotal);
    public decimal Shipping => _items.Count == 0 || Subtotal >= FreeShippingThreshold ? 0 : FlatShipping;
    public decimal Total => Subtotal + Shipping;

    public void Add(Product product, string size, string color, int quantity)
    {
        if (product.Stock <= 0) return;
        var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id && i.Size == size && i.Color == color);
        var newQty = Math.Min((existing?.Quantity ?? 0) + Math.Max(1, quantity), product.Stock);

        if (existing is null)
            _items.Add(new CartItem { Product = product, Size = size, Color = color, Quantity = newQty });
        else
            existing.Quantity = newQty;

        Changed?.Invoke();
    }

    public void SetQuantity(CartItem item, int quantity)
    {
        item.Quantity = Math.Clamp(quantity, 1, item.Product.Stock);
        Changed?.Invoke();
    }

    public void Remove(CartItem item) { _items.Remove(item); Changed?.Invoke(); }

    public void Clear() { _items.Clear(); Changed?.Invoke(); }
}