using System;

using DwFramework.Core;
using DwFramework.Core.Extensions;
using DwFramework.WebAPI.Extensions;
using DwFramework.RabbitMQ.Extensions;

namespace DwTransactionBus
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var x = new Transaction() { Operations = new[] { new Operation() { Method = "T1", Data = new { A = 1, B = "2" } } } }.ToJson();
                var a = "{\"Operations\":[{\"Method\":\"T1\",\"Data\":{\"A\":1,\"B\":\"2\"}},{\"Method\":\"T2\",\"Data\":{\"C\":1,\"D\":\"2\"}}]}".ToObject<Transaction>();

                //var host = new ServiceHost(EnvironmentType.Develop, $"{AppDomain.CurrentDomain.BaseDirectory}");
                //host.RegisterLog();
                //host.RegisterWebAPIService();
                //host.InitService(provider => provider.InitWebAPIServiceAsync<Startup>());
                //host.RegisterRabbitMQService();
                //host.RegisterFromAssemblies();
                //host.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"程序中断!:{ex.Message}");
            }
        }
    }
}
