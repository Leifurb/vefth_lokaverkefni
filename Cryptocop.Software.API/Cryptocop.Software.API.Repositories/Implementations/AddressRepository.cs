using System.Collections.Generic;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Repositories.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Cryptocop.Software.API.Models.Exceptions;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public AddressRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddAddress(string email, AddressInputModel address)
        {
            var userid = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            if (userid == null){
                throw new ResourceNotFoundException("User does not Exist with this email");
            }
            _dbContext.Address.Add(new Address{
                UserId = userid.Id,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City
            });
            _dbContext.SaveChanges();
        }

        public IEnumerable<AddressDto> GetAllAddresses(string email)
        {
            return _dbContext.Address.Include(u => u.User).Where(x => x.UserId == x.User.Id && x.User.Email == email).Select(a => new AddressDto{
                 Id = a.Id,
                 StreetName = a.StreetName,
                 HouseNumber = a.HouseNumber,
                 ZipCode = a.ZipCode,
                 Country = a.Country,
                 City = a.City
            });
        }

        public void DeleteAddress(string email, int addressId)
        {
            var entity = _dbContext.Address.Include(u => u.User).FirstOrDefault(x => x.Id == addressId && x.User.Id == x.UserId && x.User.Email == email);
            if (entity != null)
            {
                _dbContext.Address.Remove(entity);
                _dbContext.SaveChanges();
            }
            throw new ResourceNotFoundException("Address not found");
        }
    }
}