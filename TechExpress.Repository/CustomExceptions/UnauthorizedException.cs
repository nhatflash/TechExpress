using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechExpress.Repository.CustomExceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message) : base(StatusCodes.Status401Unauthorized, message)
        {
        }
    }
}
