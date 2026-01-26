using System;

namespace TechExpress.Repository.Models;

public class Brand
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? ImageUrl { get; set; }

    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;
}
