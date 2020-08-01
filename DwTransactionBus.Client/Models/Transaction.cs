using System;
using System.Collections.Generic;

using DwFramework.Core.Plugins;

namespace DwTransactionBus.Client
{
    public class Transaction
    {
        public readonly string Id;
        public readonly DateTime CreateTime;
        public readonly List<Operation> Operations = new List<Operation>();

        public Transaction()
        {
            Id = Generater.GenerateGUID().ToString();
            CreateTime = DateTime.Now;
        }

        public Transaction AddOperation(string service, string method, dynamic data)
        {
            Operations.Add(new Operation(Id, service, method, data));
            return this;
        }
    }
}
