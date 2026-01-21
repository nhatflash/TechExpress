using System;
using System.Collections.Generic;
using System.Text;

namespace TechExpress.Application.Dtos.Requests
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = default!;
    }
}
