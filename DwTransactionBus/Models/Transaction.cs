using System;
using System.Collections.Generic;

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
    }
}
