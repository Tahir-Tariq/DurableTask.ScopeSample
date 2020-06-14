using DurableTask.AzureStorage;
using DurableTask.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample
{
    class Program
    {
        int requestCount = 25;

        static async Task Main(string[] args)
        {
            Program program = new Program();

            TaskHubClient taskHubClient; TaskHubWorker taskHub;
            program.Configure(out taskHubClient, out taskHub);

            ServiceCollection collection = new ServiceCollection();
            CancellationTokenSource processingTokenSource = new CancellationTokenSource();

            program.ConfigureServices(collection);
            ServiceProvider serviceProvider = collection.BuildServiceProvider();

            program.RegisterOrchestrationAndActivities(taskHub, serviceProvider);

            await taskHub.StartAsync();
            string request = "Request";

            for (int i = 1; i <= program.requestCount; i++)
            {
                await taskHubClient.CreateOrchestrationInstanceAsync(typeof(MainOrchestration), request + i);
            }            
            
            var displayTask = Task.Factory.StartNew(() => program.Display(processingTokenSource.Token));
            
            await Task.Factory.StartNew(() => { 
                while (MainOrchestration.completedCount < program.requestCount) ; 
            });

            processingTokenSource.Cancel();
            await displayTask;
            await taskHub.StopAsync();

            Console.WriteLine("Memory used before collection:       {0:N0}", GC.GetTotalMemory(false));
            GC.Collect();
            Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));

            Console.WriteLine(ObjectsAnalyzer.GetStatsString());
            Console.ReadLine();
            
        }

        public void Display(CancellationToken token)
        {
            
            do{
                Console.CursorLeft = 0;
                Console.CursorTop = 0;

                Console.WriteLine("Processing...completed:" + MainOrchestration.completedCount);
                //Task.Delay(100).GetAwaiter().GetResult();
            } while (!token.IsCancellationRequested) ;
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ScopedOrchestration>();
            services.AddScoped<ScopedActivity>();
            services.AddTransient<TransitiveOrchestration>();
            services.AddTransient<TransitiveActivity>();
            services.AddScoped<SimpleService>();           
        }

        public void Configure(out TaskHubClient taskHubClient, out TaskHubWorker taskHub)
        {
            string storageConnectionString = "UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://127.0.0.1:10002/";

            string taskHubName = "ScopeSample";

            var orchestrationServiceAndClient =
                new AzureStorageOrchestrationService(new AzureStorageOrchestrationServiceSettings
                {
                    StorageConnectionString = storageConnectionString,
                    TaskHubName = taskHubName
                });

            taskHubClient = new TaskHubClient(orchestrationServiceAndClient);
            taskHub = new TaskHubWorker(orchestrationServiceAndClient);
            orchestrationServiceAndClient.DeleteAsync().Wait();
            orchestrationServiceAndClient.CreateIfNotExistsAsync().Wait();
        }

        public void RegisterOrchestrationAndActivities(TaskHubWorker taskHub, ServiceProvider serviceProvider)
        {
            taskHub.AddTaskOrchestrations(
                           typeof(TypedOrchestration),
                           typeof(MainOrchestration)
            );

            taskHub.AddTaskOrchestrations(
                new ScopedOrchestrationCreator<ScopedOrchestration>(serviceProvider),
                new ScopedOrchestrationCreator<TransitiveOrchestration>(serviceProvider)
            );


            taskHub.AddTaskActivities(typeof(TypedActivity));

            taskHub.AddTaskActivities(
                new ScopedActivityCreator<ScopedActivity>(serviceProvider),
                new ScopedActivityCreator<TransitiveActivity>(serviceProvider)             
            );
        }

    }


}
