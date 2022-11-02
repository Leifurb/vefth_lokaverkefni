using System.Collections.Generic;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Cryptocop.Software.API.Repositories.Entities;
using System;
namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public ShoppingCartRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            var shoppingcart = _dbContext.ShoppingCarts.Include(a => a.User).FirstOrDefault(x => x.User.Email == email);
            if (shoppingcart == null){return null;}
            var a = _dbContext.ShoppingCartItems.Where(i => i.ShoppingCartId == shoppingcart.Id).Select(s => new ShoppingCartItemDto{
                Id = shoppingcart.Id,
                ProductIdentifier = s.ProductIdentifier,
                Quantity = (float) s.Quantity,
                UnitPrice = s.UnitPrice

            });
            return a;
        }

        public void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem, float priceInUsd)
        {
            var a = shoppingCartItemItem.Quantity;
            if (a == null){
                a = (float) 0;
            }
           var userid = _dbContext.Users.FirstOrDefault(x => x.Email == email);
            var shoppingcart = _dbContext.ShoppingCarts.FirstOrDefault(s => s.UserId == userid.Id);
            if(shoppingcart == null) 
            {
                shoppingcart = new ShoppingCart
                {
                    UserId = userid.Id
                };
                _dbContext.ShoppingCarts.Add(shoppingcart);
                _dbContext.SaveChanges();
            }
            _dbContext.ShoppingCartItems.Add(new ShoppingCartItem{
                ShoppingCartId = shoppingcart.Id,
                ProductIdentifier = shoppingCartItemItem.ProductIdentifier,
                Quantity = (float) a,
                UnitPrice = priceInUsd
            });
             _dbContext.SaveChanges();
        }

        public void RemoveCartItem(string email, int id)
        {
            var shoppingCart = _dbContext.ShoppingCarts.Include(s => s.User).FirstOrDefault(u => u.User.Email == email);
            if (shoppingCart != null){  
                var entity = _dbContext.ShoppingCartItems
                                        .FirstOrDefault(i => i.ShoppingCartId == shoppingCart.Id);
                if (entity != null){
                    _dbContext.ShoppingCartItems.Remove(entity);
                    _dbContext.SaveChanges();
                }
            }
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            throw new System.NotImplementedException();
        }

        public void ClearCart(string email)
        {
            var shoppingCart = _dbContext.ShoppingCarts.Include(s => s.User).FirstOrDefault(u => u.User.Email == email);
            if (shoppingCart != null){  
                var entity = _dbContext.ShoppingCartItems
                                        .Where(i => i.ShoppingCartId == shoppingCart.Id);
                
                _dbContext.ShoppingCartItems.RemoveRange(entity);
                _dbContext.SaveChanges();
                }
        }
        public void DeleteCart(string email){
            var shoppingCart = _dbContext.ShoppingCarts.Include(s => s.User).FirstOrDefault(u => u.User.Email == email);
            if (shoppingCart != null){  
                var entity = _dbContext.ShoppingCartItems
                                        .FirstOrDefault(i => i.ShoppingCartId == shoppingCart.Id);
                if (entity != null){
                _dbContext.ShoppingCartItems.Remove(entity);}
                _dbContext.SaveChanges();
                }
        }

    }
}