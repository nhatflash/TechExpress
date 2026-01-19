using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechExpress.Repository.CustomExceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message) : base(StatusCodes.Status403Forbidden, message)
        {
        }
    }
}
