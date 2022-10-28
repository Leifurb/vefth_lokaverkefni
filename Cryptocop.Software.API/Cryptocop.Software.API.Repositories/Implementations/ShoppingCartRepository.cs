using System.Collections.Generic;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Cryptocop.Software.API.Repositories.Entities;
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
            return _dbContext.ShoppingCartItems.Where(i => i.Id == shoppingcart.Id).Select(s => new ShoppingCartItemDto{
                Id = shoppingcart.Id,
                ProductIdentifier = s.ProductIdentifier,
                Quantity = (float) s.Quantity,
                UnitPrice = s.UnitPrice

            });
        }

        public void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem, float priceInUsd)
        {
            var shoppingcart = _dbContext.ShoppingCarts.Include(u => u.User).FirstOrDefault(z => z.User.Email == email);
            _dbContext.ShoppingCartItems.Add(new ShoppingCartItem{
                ShoppingCartId = shoppingcart.Id,
                ProductIdentifier = shoppingCartItemItem.ProductIdentifier,
                Quantity = (float) shoppingCartItemItem.Quanity,
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
            throw new System.NotImplementedException();
        }

        public void DeleteCart(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}