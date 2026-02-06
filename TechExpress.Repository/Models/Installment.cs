using System;
using System.ComponentModel;
using TechExpress.Repository.Enums;

namespace TechExpress.Repository.Models;

public class Installment
{
    public Guid Id { get; set; }

    public required Guid OrderId { get; set; }

    public required int Period { get; set; }

    public required decimal Amount { get; set; }

    public required InstallmentStatus Status { get; set; }

    public required DateTimeOffset DueDate { get; set; }

    public Order Order { get; set; } = null!;
}
