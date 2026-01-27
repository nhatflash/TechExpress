using System;

namespace TechExpress.Repository.Models;

public class Cart
{
    public Guid Id { get; set; }

    public required Guid UserId { get; set; }

    public ICollection<CartItem> Items { get; set; } = [];

    public decimal TotalPrice { get; set; }

    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    public User User { get; set; } = null!;
}
