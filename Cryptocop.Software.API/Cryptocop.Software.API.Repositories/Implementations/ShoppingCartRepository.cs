using System.Collections.Generic;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Cryptocop.Software.API.Repositories.Entities;
using Cryptocop.Software.API.Models.Exceptions;
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
            if (shoppingcart == null){throw new ResourceNotFoundException("shopping cart not found");}
            var a = _dbContext.ShoppingCartItems.Where(i => i.ShoppingCartId == shoppingcart.Id).Select(s => new ShoppingCartItemDto{
                Id = shoppingcart.Id,
                ProductIdentifier = s.ProductIdentifier,
                Quantity = (float) s.Quantity,
                UnitPrice = s.UnitPrice,
                TotalPrice = (float) (s.UnitPrice * s.Quantity)

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
           if (userid == null){throw new ResourceNotFoundException("user not found");}
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
                }else{
                    throw new ResourceNotFoundException("shopping cart item not found");
                    }
            } else{
                throw new ResourceNotFoundException("shopping cart not found");}
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            var shoppingcart = _dbContext.ShoppingCarts.FirstOrDefault(u => u.User.Email == email);
            if(shoppingcart == null) {
                throw new ResourceNotFoundException("Shopping cart not found");
            }
            var shoppingcartitem = _dbContext.ShoppingCartItems.Where(i => i.ShoppingCarts.Id == shoppingcart.Id && i.Id == id).FirstOrDefault();
            if(shoppingcartitem == null) {
                throw new ResourceNotFoundException($"Shopping item with id: {id} was not found");
            }
            shoppingcartitem.Quantity = quantity;
            _dbContext.SaveChanges();
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
            else{
            throw new ResourceNotFoundException("Shopping cart not found");}
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
            else{throw new ResourceNotFoundException("Shopping cart not found");}
        }

    }
}