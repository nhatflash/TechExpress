using System;
using System.ComponentModel.DataAnnotations;

namespace TechExpress.Application.Dtos.Requests;

public class CreateProductSpecValueRequest
{
    [Required(ErrorMessage = "SpecDefinitionId không được để trống")]
    public Guid SpecDefinitionId { get; set; }

    [Required(ErrorMessage = "Giá trị Spec không được để trống")]
    public string Value { get; set; } = string.Empty;
}
