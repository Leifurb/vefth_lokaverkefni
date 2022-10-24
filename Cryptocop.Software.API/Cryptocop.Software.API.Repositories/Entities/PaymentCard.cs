
namespace Cryptocop.Software.API.Repositories.Entities
{
    public class PaymentCard
    {
        public int Id { get; set;}
        public int UserId { get; set;}
        public string CardholderName { get; set;}
        public string CardNumber { get; set;}
        public int Month { get; set;}
        public int Year { get; set;}

        public User User { get; set;}

    }
}