using System.ComponentModel.DataAnnotations;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Requests;

public class CreateSpecDefinitionRequest
{
    [Required(ErrorMessage = "Tên không được để trống")]
    [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "CategoryId không được để trống")]
    public Guid CategoryId { get; set; }

    [Required(ErrorMessage = "Đơn vị không được để trống")]
    [StringLength(20, ErrorMessage = "Đơn vị không được vượt quá 20 ký tự")]
    public string Unit { get; set; } = string.Empty;

    [Required(ErrorMessage = "Loại giá trị chấp nhận không được để trống")]
    public SpecAcceptValueType AcceptValueType { get; set; }

    [Required(ErrorMessage = "Mô tả không được để trống")]
    [StringLength(4096, ErrorMessage = "Mô tả không được vượt quá 4096 ký tự")]
    public string Description { get; set; } = string.Empty;

    public bool IsRequired { get; set; } = true;
}
