using AutoMapper;
using DataLayer;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    sealed class AppProfile :Profile
    {
      public AppProfile()
        {
            CreateMap<SignUpRequest, User>();
        }
    }
    sealed class LogInprofile :Profile
    {
        public LogInprofile()
        {
            CreateMap<LoginRequest, User>();
            CreateMap<User, LoginResponse>();   
        }
    }
}
