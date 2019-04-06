using Intro.OrleansBasics.GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
using System.Threading.Tasks;

namespace Intro.OrleansBasics.Client
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Hello grain client!");
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client=await ConnectClient())
                {
                    await DoClientWork(client);
                    Console.ReadKey();
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                // 配置silo集群标识
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "develop";
                    options.ServiceId = "OrleansBasics";
                })
                // 配置日志
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client sussessfully connected to silo host \n");

            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            var friend = client.GetGrain<IHello>(0);
            var response = await friend.SayHello($"Good morning, {friend?.GetPrimaryKeyString()}");
            Console.WriteLine($"\n\n{response}\n\n");
        }
    }
}
