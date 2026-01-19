using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechExpress.Repository.CustomExceptions
{
    public class UnauthorizedAccessException : BaseException
    {
        public UnauthorizedAccessException(string message) : base(StatusCodes.Status401Unauthorized, message)
        {
        }
    }
}
