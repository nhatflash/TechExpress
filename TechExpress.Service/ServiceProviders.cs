using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TechExpress.Repository;
using TechExpress.Repository.Models;
using TechExpress.Service.Services;
using TechExpress.Service.Utils;

namespace TechExpress.Service
{
    public class ServiceProviders
    {
        public AuthService AuthService { get; }
        public UserService UserService { get; }

        public ServiceProviders(UnitOfWork unitOfWork, JwtUtils jwtUtils, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            AuthService = new AuthService(unitOfWork, jwtUtils);
            UserService = new UserService(unitOfWork, webHostEnvironment, httpContextAccessor);
        }
    }
}
