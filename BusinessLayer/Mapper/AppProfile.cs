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
    public sealed class AppProfile :Profile
    {
      public AppProfile()
        {
            CreateMap<SignUpRequest, User>();
            CreateMap<User,SignUpResponse>();
        }
    }
    public sealed class LogInProfile :Profile
    {
        public LogInProfile()
        {
            CreateMap<LoginRequest, User>();
            CreateMap<User, LoginResponse>();   
        }
    }

    public sealed class UserProfile :Profile
    {
        public UserProfile()
        {
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }

    public sealed class UpdateProfile :Profile
    {
        public UpdateProfile()
        {
            CreateMap<UpdateUserRequest, User>();
        }
    }

    public sealed class EmployeeProfile :Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeRequest, Employee>();
            CreateMap<Employee, EmployeeResponse>();    
        }
    }

    public sealed class EmployeeSignupProfile :Profile
    {
        public EmployeeSignupProfile()
        {
            CreateMap<EmployeeSignUpRequest, Employee>();
            CreateMap<Employee, EmployeeSingUpResponse>();
        }
    }
}
