using System;

using DwFramework.Core.Plugins;

namespace DwTransactionBus
{
    public class Transaction
    {
        public readonly string Id;
        public readonly DateTime CreateTime;

        public Transaction()
        {
            Id = Generater.GenerateGUID().ToString();
            CreateTime = DateTime.Now;
        }
    }
}
