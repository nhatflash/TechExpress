using System;
using System.Collections.Generic;
using System.Text;
using TechExpress.Repository;

namespace TechExpress.Service.Services
{
    public class AuthService
    {
        private readonly UnitOfWork _unitOfWork;

        public AuthService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
