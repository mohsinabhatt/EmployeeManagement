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
    public class AdminManager
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public AdminManager(IRepository repository,IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public SignUpRequest AddUser(SignUpRequest signUpRequest)
        {
            var user = mapper.Map<User>(signUpRequest);
            user.Id = Guid.NewGuid();
            if (repository.AddAndSave(user) != 0)
                return signUpRequest;
                return null;  
        }
    }
}
