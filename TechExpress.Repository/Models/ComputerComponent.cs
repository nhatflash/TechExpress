using System;

namespace TechExpress.Repository.Models;

public class ComputerComponent
{
    public Guid Id { get; set; }

    public required Guid ComputerProductId { get; set; }

    public required Guid ComponentProductId { get; set; }

    public required int Quantity { get; set; }

    public DateTimeOffset AttachedAt { get; } = DateTimeOffset.Now;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    public Product ComputerProduct { get; set; } = null!;

    public Product ComponentProduct { get; set; } = null!;
}
