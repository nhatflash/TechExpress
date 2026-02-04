using System;
using System.ComponentModel.DataAnnotations;

namespace TechExpress.Application.Dtos.Requests;

public class CreateProductSpecValueRequest
{
    [Required(ErrorMessage = "Định nghĩa thông số không được để trống")]
    public Guid SpecDefinitionId { get; set; }

    [Required(ErrorMessage = "Giá trị Spec không được để trống")]
    public string Value { get; set; } = string.Empty;
}
