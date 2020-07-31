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
                var host = new ServiceHost(EnvironmentType.Develop, $"{AppDomain.CurrentDomain.BaseDirectory}");
                host.RegisterLog();
                host.RegisterWebAPIService();
                host.InitService(provider => provider.InitWebAPIServiceAsync<Startup>());
                host.RegisterRabbitMQService();
                host.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"程序中断!:{ex.Message}");
            }
        }
    }
}
