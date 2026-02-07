using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TechExpress.Repository.Models;
using TechExpress.Service.Constants;

namespace TechExpress.Service.Hubs;

[Authorize]
public class CartHub : Hub
{

}
