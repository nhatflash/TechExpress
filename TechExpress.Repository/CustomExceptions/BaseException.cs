using System;
using System.Collections.Generic;
using System.Text;

namespace TechExpress.Repository.CustomExceptions
{
    public class BaseException : Exception
    {
        public int StatusCode { get; set; }

        public BaseException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
