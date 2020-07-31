using System;

using Microsoft.Extensions.Logging;
using DwFramework.Core;
using DwFramework.RabbitMQ;

namespace DwTransactionBus
{
    [Registerable(typeof(BusService), Lifetime.Singleton, true)]
    public class BusService
    {
        public const string TRANSACTION_BUS_QUEUE_NAME = "queue.transactionbus";
        public const string MAP_SERVICE_EXCHANGE_NAME = "exchange.mapservice";
        public const string SERVICE_QUEUE_NAME_PREFIX = "queue.service.";

        private readonly ILogger<BusService> _logger;
        private readonly RabbitMQService _rabbitMQService;

        public BusService(ILogger<BusService> logger, RabbitMQService rabbitMQService)
        {
            _logger = logger;
            _rabbitMQService = rabbitMQService;
        }

        private void InitMQ()
        {
            _rabbitMQService.QueueDeclare(TRANSACTION_BUS_QUEUE_NAME);
            _rabbitMQService.Subscribe(TRANSACTION_BUS_QUEUE_NAME, false, CreateTransactionHandler);
            _rabbitMQService.ExchangeDeclare(MAP_SERVICE_EXCHANGE_NAME, ExchangeType.Fanout);
        }

        private void CreateServiceQueue(string serviceName)
        {
            var queueName = $"{SERVICE_QUEUE_NAME_PREFIX}{serviceName.ToLower()}";
            _rabbitMQService.QueueDeclare(queueName);
            _rabbitMQService.QueueBind(queueName, MAP_SERVICE_EXCHANGE_NAME, serviceName);
        }

        private void CreateTransactionHandler(RabbitMQ.Client.IModel model, RabbitMQ.Client.Events.BasicDeliverEventArgs args)
        {
            model.BasicAck(args.DeliveryTag, false);
        }
    }
}
