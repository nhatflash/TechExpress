using System;

namespace TechExpress.Repository.Models;

public class CartItem
{
    public Guid Id { get; set; }

    public required Guid CartId { get; set; }

    public required Guid ProductId { get; set; }

    public required int Quantity { get; set; }

    public required decimal UnitPrice { get; set; }

    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    public Cart Cart { get; set; } = null!;

    public Product Product { get; set; } = null!;
}
