using System;

namespace TechExpress.Repository.Models;

public class ProductSpecValue
{
    public long Id { get; set; }

    public required Guid ProductId { get; set; }

    public required Guid SpecDefinitionId { get; set; }

    public string? TextValue { get; set; }

    public int? NumberValue { get; set; }

    public decimal? DecimalValue { get; set; }

    public bool? BoolValue { get; set; }

    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    public Product Product { get; set; } = null!;

    public SpecDefinition SpecDefinition { get; set; } = null!;
}
