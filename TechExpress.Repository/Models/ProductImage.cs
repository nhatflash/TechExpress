using System;

namespace TechExpress.Repository.Models;

public class ProductImage
{
    public long Id { get; set; }

    public required Guid ProductId { get; set; }

    public required string ImageUrl { get; set; }

    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

    public Product Product { get; set; } = null!;
}
