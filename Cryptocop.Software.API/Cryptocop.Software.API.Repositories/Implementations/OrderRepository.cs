using System;
using System.Collections.Generic;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Models.Exceptions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Cryptocop.Software.API.Repositories.Entities;
using Cryptocop.Software.API.Repositories.Helpers;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public OrderRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private List<OrderItemDto> GetAllOrderItemsHelper(int orderId){
                return _dbContext.OrderItems.Where(i => orderId == i.OrderId).Select(n => new OrderItemDto{
                                                Id = n.Id,
                                                ProductIdentifier = n.ProductIdentifier,
                                                Quantity = n.Quantity,
                                                UnitPrice = n.UnitPrice,
                                                TotalPrice = n.TotalPrice}).ToList();
                }
        public IEnumerable<OrderDto> GetOrders(string email)
        {
            var userid = _dbContext.Users.Where(u => u.Email == email).Select(b => b.Id);
            if (userid == null){throw new ResourceExistsException("User does not exist");}
            var orders = _dbContext.Orders
                                .Include(m => m.OrderItem)
                                .Where(a => a.Email == email)
                                .Select(b => new OrderDto{
                                        Id = b.Id,
                                        Email = b.Email,
                                        FullName = b.FullName,
                                        StreetName = b.StreetName,
                                        HouseNumber = b.HouseNumber,
                                        ZipCode = b.ZipCode,
                                        Country = b.Country,
                                        City = b.City,
                                        CardholderName = b.CardholderName,
                                        CreditCard = b.MaskedCreditCard,
                                        OrderDate = b.OrderDate.ToString("dd'.'MM'.'yyyy"),
                                        TotalPrice = b.TotalPrice,
                                        OrderItems = _dbContext.OrderItems
                                                                .Where(i => b.Id == i.OrderId)
                                                                .Select(n => new OrderItemDto{
                                                                        Id = n.Id,
                                                                        ProductIdentifier = n.ProductIdentifier,
                                                                        Quantity = n.Quantity,
                                                                        UnitPrice = n.UnitPrice,
                                                                        TotalPrice = n.TotalPrice}).ToList()});
            return orders;
        }

        public OrderDto CreateNewOrder(string email, OrderInputModel order)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            var address = _dbContext.Address.FirstOrDefault(x => x.Id == order.AddressId);
            var payment = _dbContext.PaymentCards.FirstOrDefault(x => x.Id == order.PaymentCardId);
            var shoppingcart = _dbContext.ShoppingCarts.FirstOrDefault(x => x.UserId == user.Id);
            if (user==null){
                throw new ResourceNotFoundException("User does not exist");
            }
            if (address==null){
                throw new ResourceNotFoundException("Address not found");
            }
            if (payment==null){
                throw new ResourceNotFoundException("Payment card not found");
            } 
            
            if (shoppingcart == null){
                throw new ResourceNotFoundException("Shopping cart not found");
            }
            var shoppingCartItems = _dbContext.ShoppingCartItems.Where(i => i.ShoppingCartId == shoppingcart.Id);
            float total_in_cart = 0;
            foreach(var i in shoppingCartItems){
                total_in_cart += i.Quantity*i.UnitPrice;
            }//.Sum(z => z.Quantity*z.UnitPrice);

            var neworder = new Order{
                Email = user.Email,
                FullName = user.FullName,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City,
                CardholderName = payment.CardholderName,
                MaskedCreditCard = PaymentCardHelper.MaskPaymentCard(payment.CardNumber),
                OrderDate = DateTime.UtcNow,
                TotalPrice = total_in_cart,
                UserId = user.Id
               };
            
            _dbContext.Orders.Add(neworder);
            _dbContext.SaveChanges();
            foreach(var item in _dbContext.ShoppingCartItems.Where(i => i.ShoppingCartId == shoppingcart.Id).ToList()){
                _dbContext.OrderItems.Add(new OrderItem{
                    OrderId = neworder.Id,
                    ProductIdentifier = item.ProductIdentifier,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.Quantity * item.UnitPrice
                });
            _dbContext.SaveChanges();
            }

            var retorder =  new OrderDto{
                Id = neworder.Id,
                Email = neworder.Email,
                FullName = neworder.FullName,
                StreetName = neworder.StreetName,
                HouseNumber = neworder.HouseNumber,
                ZipCode = neworder.ZipCode,
                Country = neworder.Country,
                City = neworder.City,
                CardholderName = neworder.CardholderName,
                CreditCard = payment.CardNumber,
                OrderDate = neworder.OrderDate.ToString("dd'.'MM'.'yyyy"),
                TotalPrice = neworder.TotalPrice,
                OrderItems = null
            };
            retorder.OrderItems = GetAllOrderItemsHelper(neworder.Id);
            return retorder;
        }
    }
}