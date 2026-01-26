using System;

namespace TechExpress.Repository.Models;

public class BrandCategory
{
    public long Id { get; set; }

    public required Guid CategoryId { get; set; }

    public required Guid BrandId { get; set; }

    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

    public Category Category { get; set; } = null!; 

    public Brand Brand { get; set; } = null!;
}
