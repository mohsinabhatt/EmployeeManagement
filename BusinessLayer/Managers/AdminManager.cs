using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class AdminManager
    {
        private readonly IMapper mapper;
        private readonly AdminRepository adminRepository;

        public AdminManager(IMapper mapper,AdminRepository adminRepository)
        {
            this.mapper = mapper;
            this.adminRepository = adminRepository;
        }

        public SignUpResponse AddUser(SignUpRequest signUpRequest)
        {
            SignUpResponse signUpResponse = new SignUpResponse();
            var user = mapper.Map<User>(signUpRequest);
            user.Salt = AppEncryption.CreateSalt();
            user.IsActive = true;
            user.IsDeleted = false;
            user.Id = Guid.NewGuid();
            user.Password = AppEncryption.CreatePasswordHash(signUpRequest.Password, user.Salt);
            if (adminRepository.AddAndSave(user) != 0)
                signUpResponse.Id = user.Id;
                signUpResponse.Email = user.Email;
                signUpResponse.UserRole = user.UserRole;
                signUpResponse.ContactNo = user.ContactNo;
                signUpResponse.Gender = user.Gender;
            //return mapper.Map<User,SignUpResponse>(user);
            return signUpResponse;
            return null;  
        }

        public IEnumerable<UserResponse> GetAll()
        {
           return  adminRepository.GetAllUsers();
        }


        public UserResponse GetById(Guid id)
        {
            return mapper.Map<UserResponse>(adminRepository.GetById<User>(id));
        }


        public UpdateUserRequest Update(UpdateUserRequest updateUserRequest)

        {
           var user = mapper.Map<User>(updateUserRequest);
           if(adminRepository.UpdateAndSave(user) != 0)
            {
                return updateUserRequest;
            }
            return null;
        }

        public User Delete(Guid id)
        {
            //return adminRepository.DeleteUser(id);
            var user =  adminRepository.GetById<User>(id);
            user.IsDeleted = true;
            if (adminRepository.UpdateAndSave(user) != 0)
                return user;
                return null;
        }
     
    }
}
