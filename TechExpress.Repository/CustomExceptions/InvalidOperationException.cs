using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechExpress.Repository.CustomExceptions
{
    public class InvalidOperationException : BaseException
    {
        public InvalidOperationException(string message) : base(StatusCodes.Status400BadRequest, message)
        {
        }
    }
}
