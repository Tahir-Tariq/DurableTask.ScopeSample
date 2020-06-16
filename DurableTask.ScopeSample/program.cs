using DurableTask.Core;
using DurableTask.ScopeSample.Orchestrations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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

            int requestCount = 1;

            await taskHub.StartAsync();
            string request = "Request";

            var displayTask = Task.Factory.StartNew(() => Display(processingTokenSource.Token));
            var orchestrationList = new List<OrchestrationInstance>();
            for (int i = 1; i <= requestCount; i++)
            {
                var instance = await taskHubClient.CreateOrchestrationInstanceAsync(typeof(MainOrchestration), request + i);

                orchestrationList.Add(instance);
            }

            await Task.Factory.StartNew(() => { 
                while (MainOrchestration.completedCount < requestCount) ; 
            });

            processingTokenSource.Cancel();
            await displayTask;

            bool running = true;
            while (running)
            {
                foreach (var oi in orchestrationList)
                {
                    running = false;

                    var state = await taskHubClient.GetOrchestrationStateAsync(oi);

                    if (state.OrchestrationStatus != OrchestrationStatus.Completed)
                        running = true;
                    if (state.OrchestrationStatus == OrchestrationStatus.Completed)
                        Console.WriteLine("'{0}'=> {1}", state.Input, state.Output);
                }
            }            

            await taskHub.StopAsync();
            taskHub.Dispose();
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
