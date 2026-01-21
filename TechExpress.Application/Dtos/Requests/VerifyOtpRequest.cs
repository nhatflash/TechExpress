using System;
using System.Collections.Generic;
using System.Text;

namespace TechExpress.Application.Dtos.Requests
{
    public class VerifyOtpDto
    {
        public string Email { get; set; } = default!;
        public string Otp { get; set; } = default!;
    }
}
