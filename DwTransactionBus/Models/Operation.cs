using System;

using DwFramework.Core.Plugins;

namespace DwTransactionBus
{
    public class Operation
    {
        public string Id;
        public string Service { get; set; }
        public string Method { get; set; }
        public dynamic Data { get; set; }
        public bool IsFinished = false;

        public Operation()
        {
            Id = Generater.GenerateGUID().ToString();
        }
    }

    public class FinishedOperation
    {
        public string TransactionId { get; set; }
        public string OperationId { get; set; }
    }
}
