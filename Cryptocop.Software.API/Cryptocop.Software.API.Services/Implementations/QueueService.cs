using Cryptocop.Software.API.Services.Interfaces;
using Cryptocop.Software.API.Models.Dtos;
using System;
using Cryptocop.Software.API.Services.Helpers;

using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
namespace Cryptocop.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchange;

        public QueueService(IConfiguration configuration) 
        {
            var host = "rabbitmq";
            _exchange = "order_exchange";

            var factory = new ConnectionFactory() { HostName = host };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_exchange, "topic", true);
        }
        public void PublishMessage(string routingKey, object body)
        {
            _channel.BasicPublish(_exchange, routingKey, body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body)));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            _channel.Dispose();
            _connection.Dispose();
        }
    }
}