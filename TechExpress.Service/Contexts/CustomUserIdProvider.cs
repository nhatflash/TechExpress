using System;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace TechExpress.Service.Contexts;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
