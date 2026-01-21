using System;
using System.Collections.Generic;
using System.Text;

namespace TechExpress.Service.Dtos
{
    public class ForgotPasswordOtpCache
    {
        public string OtpHash { get; set; } = default!;
    }
}
