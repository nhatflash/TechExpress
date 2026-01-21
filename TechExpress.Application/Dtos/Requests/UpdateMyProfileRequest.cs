using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Requests
{
    public class UpdateMyProfileRequest
    {
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự.")]
        public string? Phone { get; set; }

        public Gender? Gender { get; set; }

        [StringLength(256, ErrorMessage = "Tỉnh / Thành phố không được vượt quá 256 ký tự.")]
        public string? Province { get; set; }

        [StringLength(100, ErrorMessage = "Phường / Xã không được vượt quá 100 ký tự.")]
        public string? Ward { get; set; }

        [StringLength(256, ErrorMessage = "Địa chỉ không được vượt quá 256 ký tự.")]
        public string? StreetAddress { get; set; }
    }
}
