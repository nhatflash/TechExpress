using System;
using TechExpress.Repository.Enums;

namespace TechExpress.Repository.Models;

public class SpecDefinition
{
    public Guid Id { get; set; }
    
    public string Code { get; set;  } = string.Empty;

    public required string Name { get; set; }

    public required Guid CategoryId { get; set; }

    public required string Unit { get; set; }

    public required SpecAcceptValueType AcceptValueType { get; set; }

    public required string Description { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsRequired { get; set; }

    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    public Category Category { get; set; } = null!;
}
