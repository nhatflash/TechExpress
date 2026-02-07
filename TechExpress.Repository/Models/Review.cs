using System;

namespace TechExpress.Repository.Models;

public class Review
{
    public Guid Id { get; set; }

    public required Guid ProductId { get; set; }

    public Guid? UserId { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public required string Comment { get; set; }

    public required int Rating { get; set; }

    public ICollection<ReviewMedia> Medias { get; set; } = [];

    public bool IsDeleted { get; set; }

    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    public Product Product { get; set; } = null!;

    public User? User { get; set; }
}
