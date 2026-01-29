using System.ComponentModel.DataAnnotations;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Requests;

public class UpdateSpecDefinitionRequest
{
    [StringLength(100, ErrorMessage = "Code không được vượt quá 100 ký tự")]
    public string? Code { get; set; }

    [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
    public string? Name { get; set; }

    public Guid? CategoryId { get; set; }

    [StringLength(20, ErrorMessage = "Đơn vị không được vượt quá 20 ký tự")]
    public string? Unit { get; set; }

    public SpecAcceptValueType? AcceptValueType { get; set; }

    [StringLength(4096, ErrorMessage = "Mô tả không được vượt quá 4096 ký tự")]
    public string? Description { get; set; }

    public bool? IsRequired { get; set; }
}
