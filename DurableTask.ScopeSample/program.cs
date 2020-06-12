using DurableTask.AzureStorage;
using DurableTask.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample
{
    class Program
    {
        
        static async Task Main(string[] args)
        {
            Program program = new Program();

            TaskHubClient taskHubClient; TaskHubWorker taskHub;
            program.Configure(out taskHubClient, out taskHub);

            ServiceCollection collection = new ServiceCollection();
            program.ConfigureServices(collection);
            ServiceProvider serviceProvider = collection.BuildServiceProvider();

            program.RegisterOrchestrationAndActivities(taskHub, serviceProvider);

            await taskHub.StartAsync();
            string request = "Request";

            for (int i = 1; i <= 2; i++)
            {
                await taskHubClient.CreateOrchestrationInstanceAsync(typeof(MainOrchestration), request + i);
            }


            Console.WriteLine("Press enter to close");
            Console.ReadLine();

            Console.WriteLine("Memory used before collection:       {0:N0}",
                       GC.GetTotalMemory(false));

            // Collect all generations of memory.
            GC.Collect();
            Console.WriteLine("Memory used after full collection:   {0:N0}",
                              GC.GetTotalMemory(true));

            await taskHub.StopAsync();
            Console.WriteLine("Task hub is stopped");
            Console.ReadLine();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ScopedOrchestration>();
            services.AddScoped<ScopedActivity>();
            services.AddTransient<TransitiveOrchestration>();
            services.AddTransient<TransitiveActivity>();
            services.AddScoped<SimpleService>();
            services.AddScoped<ProxyActivity>();

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
            //orchestrationServiceAndClient.DeleteAsync().Wait();
            orchestrationServiceAndClient.CreateIfNotExistsAsync().Wait();
        }

        public void RegisterOrchestrationAndActivities(TaskHubWorker taskHub, ServiceProvider serviceProvider)
        {
            taskHub.AddTaskOrchestrations(
                           typeof(TypedOrchestration),
                           typeof(MainOrchestration)
            );

            taskHub.AddTaskOrchestrations(
                ServiceProviderObjectCreator.CreateOrchestrationCreator<ScopedOrchestration>(serviceProvider),
                ServiceProviderObjectCreator.CreateOrchestrationCreator<TransitiveOrchestration>(serviceProvider)
            );

            taskHub.AddOrchestrationDispatcherMiddleware((context, next) =>{
                var orchestration = context.GetProperty<TaskOrchestration>();

                if (orchestration is IMyIdentity identity)
                {
                    //Console.WriteLine("Middleware :" + identity.MyIdentity);
                } 

                return next();
            });


            taskHub.AddActivityDispatcherMiddleware((context, next) =>
            {
                var activity = context.GetProperty<TaskActivity>();

                if (activity is IMyIdentity identity)
                {
                    //Console.WriteLine("Middleware :" + identity.MyIdentity);
                }                   

                return next();
            });

            taskHub.AddTaskActivities(typeof(TypedActivity));


            taskHub.AddTaskActivities(
                ServiceProviderObjectCreator.CreateActivityCreator<ScopedActivity>(serviceProvider),
                ServiceProviderObjectCreator.CreateActivityCreator<TransitiveActivity>(serviceProvider),
                ServiceProviderObjectCreator.CreateActivityCreator<ProxyActivity>(serviceProvider)
            );
        }

    }


}
