using System;
using System.Text;

using Microsoft.Extensions.Logging;
using DwFramework.Core.Extensions;
using DwFramework.RabbitMQ;

namespace DwTransactionBus.Client
{
    public class BusClient
    {
        public const string CREAT_TRANSACTION_QUEUE_NAME = "TransactionBus.CreateTransaction";
        public const string FINISH_OPERATION_QUEUE_NAME = "TransactionBus.FinishOperation";
        public const string MAP_SERVICE_EXCHANGE_NAME = "TransactionBus.MapService";
        public const string SERVICE_QUEUE_NAME_PREFIX = "TransactionBus.Service.";

        private readonly ILogger<BusClient> _logger;
        private readonly RabbitMQService _rabbitMQService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rabbitMQService"></param>
        public BusClient(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="rabbitMQService"></param>
        public BusClient(ILogger<BusClient> logger, RabbitMQService rabbitMQService) : this(rabbitMQService)
        {
            _logger = logger;
        }

        /// <summary>
        /// 发布事务
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public string PublishTransaction(Transaction transaction)
        {
            _rabbitMQService.Publish(transaction, routingKey: CREAT_TRANSACTION_QUEUE_NAME);
            return transaction.Id;
        }

        /// <summary>
        /// 订阅事务
        /// </summary>
        /// <param name="service"></param>
        /// <param name="handler"></param>
        public void SubscribeTransaction(string service, Func<Operation, bool> handler)
        {
            var queueName = $"{SERVICE_QUEUE_NAME_PREFIX}{service}";
            _rabbitMQService.QueueDeclare(queueName);
            _rabbitMQService.QueueBind(queueName, MAP_SERVICE_EXCHANGE_NAME);
            _rabbitMQService.Subscribe(queueName, false, (model, args) =>
            {
                var msg = Encoding.UTF8.GetString(args.Body.ToArray());
                var operation = msg.ToObject<Operation>();
                if (handler(operation)) _rabbitMQService.Publish(new FinishedOperation() { TransactionId = operation.TransactionId, OperationId = operation.OperationId }, routingKey: FINISH_OPERATION_QUEUE_NAME);
                model.BasicAck(args.DeliveryTag, false);
            });
        }
    }
}
