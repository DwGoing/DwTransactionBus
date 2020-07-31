using System;

namespace DwTransactionBus
{
    public class Operation
    {
        public string Service { get; set; }
        public string Method { get; set; }
        public dynamic Data { get; set; }
    }
}
