using AutoMapper;
using DataLayer;
using ModelLayer;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class AccountManager
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public AccountManager(IRepository repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public LoginResponse LogIn(LoginRequest loginRequest)
        {
          var user = repository.FindBy<User>(x => x.Email == loginRequest.Email && x.IsDeleted == false).FirstOrDefault();
            if (user != null)
            {
                if(user.Password.Equals(AppEncryption.CreatePasswordHash(loginRequest.password,user.Salt)))
                    return mapper.Map<LoginResponse>(user); 
            }
            return null;
        
        }
    }
}