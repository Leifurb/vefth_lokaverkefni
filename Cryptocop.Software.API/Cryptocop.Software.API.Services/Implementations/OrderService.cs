using System.Collections.Generic;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;

using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingcartRepository;

        private readonly IQueueService _queueService;

        public OrderService(IOrderRepository orderRepository, IShoppingCartRepository shoppingcartRepository, IQueueService queueService)
        {
            _orderRepository = orderRepository;
            _shoppingcartRepository = shoppingcartRepository;
            _queueService = queueService;
        }
        public IEnumerable<OrderDto> GetOrders(string email)
        {
            return _orderRepository.GetOrders(email);
        }

        public void CreateNewOrder(string email, OrderInputModel order)
        {
            var ret = _orderRepository.CreateNewOrder(email, order);
            _shoppingcartRepository.ClearCart(email);
            _shoppingcartRepository.DeleteCart(email);
            _queueService.PublishMessage("create-order", ret);
  
            
        }
    }
}