using System;
using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Repositories.Entities;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Models.Exceptions;
namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public UserRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserDto CreateUser(RegisterInputModel inputModel)
        {
            if(_dbContext.Users.Where(x => x.Email == inputModel.Email).Count() > 0)
            {
                throw new ResourceExistsException("User with email alredy exists");
            }
            else{
                var user = new User{
                    FullName = inputModel.FullName,
                    Email = inputModel.Email,
                    HashedPassword = HashingHelper.HashPassword(inputModel.Password)

                };
                _dbContext.Users.Add(user);
                
                var token = new JwtToken{
                    Blacklisted = false
                };
                _dbContext.JwtTokens.Add(token);

                _dbContext.SaveChanges();
                
                return new UserDto{
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    TokenId = token.Id
                };
            }  
        }

        public UserDto AuthenticateUser(LoginInputModel loginInputModel)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == loginInputModel.Email);
            if (user == null){ throw new NotAuthorized("wrong email");}

            if (user.HashedPassword != HashingHelper.HashPassword(loginInputModel.Password)){
                throw new NotAuthorized("wrong password");

            }
            var token = new JwtToken();
            _dbContext.JwtTokens.Add(token);
            _dbContext.SaveChanges();
                return new UserDto{
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                TokenId = token.Id
                };
        }
        
    }
}