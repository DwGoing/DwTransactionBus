namespace DwTransactionBus
{
    public class Operation
    {
        public string Id { get; set; }
        public string Service { get; set; }
        public string Method { get; set; }
        public dynamic Data { get; set; }
        public bool IsFinished = false;

    }

    public class FinishedOperation
    {
        public string TransactionId { get; set; }
        public string OperationId { get; set; }
    }
}
