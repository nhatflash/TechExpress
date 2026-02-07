using System;

namespace TechExpress.Repository.Models;

public class ReviewMedia
{
    public long Id { get; set; }

    public required Guid ReviewId { get; set; }

    public required string? MediaUrl { get; set; }

    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

    public Review Review { get; set; } = null!;
}
