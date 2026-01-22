using System;
using Microsoft.AspNetCore.Http;

namespace TechExpress.Repository.CustomExceptions;

public class ServiceUnavailableException : BaseException
{
    public ServiceUnavailableException(string message) : base(StatusCodes.Status503ServiceUnavailable, message)
    {
    }
}
