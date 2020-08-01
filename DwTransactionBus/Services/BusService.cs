using System;
using System.Text;

using Microsoft.Extensions.Logging;
using DwFramework.Core;
using DwFramework.Core.Extensions;
using DwFramework.Core.Plugins;
using DwFramework.RabbitMQ;

namespace DwTransactionBus
{
    [Registerable(typeof(BusService), Lifetime.Singleton, true)]
    public class BusService
    {
        public const string CREAT_TRANSACTION_QUEUE_NAME = "TransactionBus.CreateTransaction";
        public const string FINISH_OPERATION_QUEUE_NAME = "TransactionBus.FinishOperation";
        public const string MAP_SERVICE_EXCHANGE_NAME = "TransactionBus.MapService";
        public const string SERVICE_QUEUE_NAME_PREFIX = "TransactionBus.Service.";

        private readonly ILogger<BusService> _logger;
        private readonly MemoryCache _memoryCache;
        private readonly RabbitMQService _rabbitMQService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="memoryCache"></param>
        /// <param name="rabbitMQService"></param>
        public BusService(ILogger<BusService> logger, MemoryCache memoryCache, RabbitMQService rabbitMQService)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _rabbitMQService = rabbitMQService;
            InitMQ();
        }

        /// <summary>
        /// 初始化MQ
        /// </summary>
        private void InitMQ()
        {
            _rabbitMQService.QueueDeclare(CREAT_TRANSACTION_QUEUE_NAME);
            _rabbitMQService.QueueDeclare(FINISH_OPERATION_QUEUE_NAME);
            _rabbitMQService.Subscribe(CREAT_TRANSACTION_QUEUE_NAME, false, CreateTransactionHandler);
            _rabbitMQService.Subscribe(FINISH_OPERATION_QUEUE_NAME, false, FinishOperationHandler);
            _rabbitMQService.ExchangeDeclare(MAP_SERVICE_EXCHANGE_NAME, ExchangeType.Fanout);
        }

        /// <summary>
        /// 创建事务
        /// </summary>
        /// <param name="model"></param>
        /// <param name="args"></param>
        private void CreateTransactionHandler(RabbitMQ.Client.IModel model, RabbitMQ.Client.Events.BasicDeliverEventArgs args)
        {
            var msg = Encoding.UTF8.GetString(args.Body.ToArray());
            var transaction = msg.ToObject<Transaction>();
            _logger.LogInformation($"创建事务 => {transaction.ToJson()}");
            _memoryCache.Set(transaction.Id, transaction);
            model.BasicAck(args.DeliveryTag, false);
        }

        /// <summary>
        /// 完成操作
        /// </summary>
        /// <param name="model"></param>
        /// <param name="args"></param>
        private void FinishOperationHandler(RabbitMQ.Client.IModel model, RabbitMQ.Client.Events.BasicDeliverEventArgs args)
        {
            var msg = Encoding.UTF8.GetString(args.Body.ToArray());
            var operation = msg.ToObject<FinishedOperation>();
            _logger.LogInformation($"完成操作 => {operation.ToJson()}");
            var transaction = _memoryCache.Get<Transaction>(operation.TransactionId);
            if (transaction == null) return;
            transaction.FinishOperation(operation.OperationId);
            if (transaction.IsFinished()) _memoryCache.Del(transaction.Id);
            model.BasicAck(args.DeliveryTag, false);
        }
    }
}
