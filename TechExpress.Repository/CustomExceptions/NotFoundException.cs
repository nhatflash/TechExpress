using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechExpress.Repository.CustomExceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(StatusCodes.Status404NotFound, message)
        {
        }
    }
}
