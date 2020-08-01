using System;
using System.Linq;

using DwFramework.Core.Plugins;

namespace DwTransactionBus
{
    public class Transaction
    {
        public readonly string Id;
        public readonly DateTime CreateTime;
        public Operation[] Operations { get; set; }

        public Transaction()
        {
            Id = Generater.GenerateGUID().ToString();
            CreateTime = DateTime.Now;
        }

        public void FinishOperation(string operationId)
        {
            var operation = Operations.Where(item => item.Id == operationId).SingleOrDefault();
            if (operation != null) operation.IsFinished = true;
        }

        public bool IsFinished()
        {
            return !Operations.Where(item => item.IsFinished == false).Any();
        }
    }
}
