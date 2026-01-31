using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechExpress.Application.Dtos.Requests;

public class UpdateProductImagesRequest
{
    [Required(ErrorMessage = "ProductId không được để trống")]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Danh sách URL ảnh mới. Nếu null hoặc rỗng thì sẽ xóa toàn bộ ảnh hiện tại.
    /// </summary>
    public List<string>? Images { get; set; }
}

