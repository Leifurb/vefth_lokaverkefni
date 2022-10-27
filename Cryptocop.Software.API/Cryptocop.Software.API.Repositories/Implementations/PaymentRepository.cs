using System.Collections.Generic;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;

using System.Linq;
using Cryptocop.Software.API.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public PaymentRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddPaymentCard(string email, PaymentCardInputModel paymentCard)
        {
            //checks if card already is in database and belongs to user otherwise add card to db.
            if (_dbContext.PaymentCards.Include(u => u.User)
                                       .FirstOrDefault(x => x.CardNumber == paymentCard.CardNumber && x.User.Email == email) != null)
                        {
                        var userid = _dbContext.Users.FirstOrDefault(x => x.Email == email);
                        _dbContext.PaymentCards.Add(new PaymentCard{
                            UserId = userid.Id,
                            CardholderName = paymentCard.CardholderName,
                            CardNumber = paymentCard.CardNumber,
                            Month = paymentCard.Month,
                            Year = paymentCard.Year
                        });
                        _dbContext.SaveChanges();

                        }

            
        }

        public IEnumerable<PaymentCardDto> GetStoredPaymentCards(string email)
        {
          return _dbContext.PaymentCards.Include(u => u.User).Where(x => x.UserId == x.User.Id && x.User.Email == email).Select(a => new PaymentCardDto{
                    Id = a.Id,
                    CardholderName = a.CardholderName,
                    CardNumber = a.CardNumber,
                    Month = a.Month,
                    Year = a.Year
            });
        }
    }
}