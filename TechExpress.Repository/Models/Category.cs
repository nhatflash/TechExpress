using System;
using TechExpress.Repository.Enums;

namespace TechExpress.Repository.Models;

public class Category
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public Guid? ParentCategoryId { get; set; }

    public required string Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    public Category? ParentCategory { get; set; }
}
