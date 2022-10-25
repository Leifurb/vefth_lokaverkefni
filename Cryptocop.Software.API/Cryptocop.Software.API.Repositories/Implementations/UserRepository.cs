using System;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using System.Linq;
using Cryptocop.Software.API.Repositories.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
         private string _salt = "00209b47-08d7-475d-a0fb-20abf0872ba0";
        private readonly CryptocopDbContext _dbContext;

        public UserRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public UserDto CreateUser(RegisterInputModel inputModel)
        {
            if(_dbContext.Users.Where(x => x.Email == inputModel.Email).FirstOrDefault() == null)
            {
                throw new Exception();
            }
            else{
                var user = new User{
                    FullName = inputModel.FullName,
                    Email = inputModel.Email,
                    HashedPassword = HashPassword(inputModel.Password)

                };
                var token = new JwtToken();
                _dbContext.JwtTokens.Add(token);
                _dbContext.Users.Add(user);
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
            if (user == null){return null;}

            if (user.HashedPassword == HashPassword(loginInputModel.Password)){
                var token = new JwtToken();
                _dbContext.JwtTokens.Add(token);
                _dbContext.SaveChanges();
                 return new UserDto{
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    TokenId = token.Id

                };
            }else{
                return null; //wrong password must remember to add custom error
            }
        }
                    
            
        private string HashPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: CreateSalt(),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        }

        private byte[] CreateSalt() =>
            Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(_salt)));
        
    }
}