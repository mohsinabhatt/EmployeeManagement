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
          var user = repository.FindBy<User>(x => x.Email == loginRequest.Email && x.Password == loginRequest.password);
            if (user != null)
            {
                return mapper.Map<LoginResponse>(user);
            }
            return null;
        }
    }
}
