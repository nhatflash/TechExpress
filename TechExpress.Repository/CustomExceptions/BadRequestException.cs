using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechExpress.Repository.CustomExceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(StatusCodes.Status400BadRequest, message)
        {
        }
    }
}
