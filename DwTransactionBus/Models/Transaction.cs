using System;
using System.Linq;

namespace DwTransactionBus
{
    public class Transaction
    {
        public string Id { get; set; }
        public DateTime CreateTime { get; set; }
        public Operation[] Operations { get; set; }

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
