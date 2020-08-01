using DwFramework.Core.Plugins;

namespace DwTransactionBus.Client
{
    public class Operation
    {
        public readonly string TransactionId;
        public readonly string OperationId;
        public readonly string Service;
        public readonly string Method;
        public readonly dynamic Data;

        public Operation(string transactionId, string service, string method, dynamic data)
        {
            TransactionId = transactionId;
            OperationId = Generater.GenerateGUID().ToString();
            Service = service;
            Method = method;
            Data = data;
        }
    }

    public class FinishedOperation
    {
        public string TransactionId { get; set; }
        public string OperationId { get; set; }
    }
}
