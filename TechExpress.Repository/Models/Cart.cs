using System;
using TechExpress.Repository.Enums;

namespace TechExpress.Repository.Models;

public class Cart
{
    public Guid Id { get; set; }

    public required Guid UserId { get; set; }

    public required CartStatus Status { get; set; }

    public ICollection<CartItem> Items { get; set; } = [];

    public decimal TotalPrice { get; set; }

    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    public User User { get; set; } = null!;

    public void RecalculateTotalPrice()
    {
        TotalPrice = Items.Sum(item => item.Quantity * item.UnitPrice);
        UpdatedAt = DateTimeOffset.Now;
    }
}
