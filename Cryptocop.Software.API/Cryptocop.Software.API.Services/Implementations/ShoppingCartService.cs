using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
namespace Cryptocop.Software.API.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingcartRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingcartRepository)
        {
            _shoppingcartRepository = shoppingcartRepository;
        }

        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            return _shoppingcartRepository.GetCartItems(email);
        }

        public async Task AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem)
        {
                HttpClient client = new HttpClient();
                string assetKey = shoppingCartItemItem.ProductIdentifier;
                client.DefaultRequestHeaders.Add("x-messari-api-key", "7c5f8d9e-ea09-404d-abfe-f8a39bd9ee2a");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("https://data.messari.io/api/v1/assets/" + assetKey + "/metrics" + "?fields=id,name,slug,symbol,market_data/price_usd");
                var json = await response.Content.ReadAsStringAsync();
                var res = await HttpResponseMessageExtensions.DeserializeJsonToObject<CryptoCurrencyDto>(response, true);
                _shoppingcartRepository.AddCartItem(email, shoppingCartItemItem, res.PricceInUsd);
        }

        public void RemoveCartItem(string email, int id)
        {
            _shoppingcartRepository.RemoveCartItem(email,id);
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            _shoppingcartRepository.UpdateCartItemQuantity(email, id, quantity);
        }

        public void ClearCart(string email)
        {
            _shoppingcartRepository.ClearCart(email);
        }
    }
}
