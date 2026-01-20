using System;
using System.Collections.Generic;
using System.Text;

namespace TechExpress.Service.Dtos.Requests
{
    public class ForgotPasswordRequestDto
    {
        public string Email { get; set; } = default!;
    }
}
