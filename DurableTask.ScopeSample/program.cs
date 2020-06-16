using DurableTask.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample
{
    class Program
    {        
        static async Task Main(string[] args)
        {
            DurableService durableService = new DurableService();

            TaskHubClient taskHubClient; TaskHubWorker taskHub;
            durableService.Configure(out taskHubClient, out taskHub);

            ServiceCollection collection = new ServiceCollection();
            CancellationTokenSource processingTokenSource = new CancellationTokenSource();

            durableService.ConfigureServices(collection);
            ServiceProvider serviceProvider = collection.BuildServiceProvider();

            durableService.RegisterOrchestrationAndActivities(taskHub, serviceProvider);

            int requestCount = 25;

            await taskHub.StartAsync();
            string request = "Request";

            var displayTask = Task.Factory.StartNew(() => Display(processingTokenSource.Token));

            for (int i = 1; i <= requestCount; i++)
            {
                await taskHubClient.CreateOrchestrationInstanceAsync(typeof(MainOrchestration), request + i);
            }

            await Task.Factory.StartNew(() => { 
                while (MainOrchestration.completedCount < requestCount) ; 
            });

            processingTokenSource.Cancel();
            await displayTask;
            await taskHub.StopAsync();
            taskHub.Dispose();

            Console.WriteLine("Memory used before collection:       {0:N0}", GC.GetTotalMemory(false));
            GC.Collect();
            Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));

            Console.WriteLine(ObjectsAnalyzer.GetStatsString());
            Console.ReadLine();
            
        }

        public static void Display(CancellationToken token)
        {
            
            do{
                Console.CursorLeft = 0;
                Console.CursorTop = 0;

                Console.WriteLine("Processing...completed:" + MainOrchestration.completedCount);
                
            } while (!token.IsCancellationRequested) ;
            
        }      
    }
}
