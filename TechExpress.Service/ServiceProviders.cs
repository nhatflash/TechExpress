using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository;
using TechExpress.Service.Services;

namespace TechExpress.Service
{
    public class ServiceProviders
    {
        public AuthService AuthService { get; }

        public ServiceProviders(UnitOfWork unitOfWork)
        {
            AuthService = new AuthService(unitOfWork);
        }
    }
}
