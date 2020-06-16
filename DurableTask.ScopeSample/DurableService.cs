using DurableTask.AzureStorage;
using DurableTask.Core;
using DurableTask.ScopeSample.Orchestrations;
using DurableTask.ScopeSample.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DurableTask.ScopeSample
{
    public class DurableService
    {        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DummyOrchestration>();
            services.AddScoped<DummyActivity>();
            services.AddScoped<ScopedOrchestration>();
            services.AddScoped<ScopedActivity>();
            services.AddTransient<TransitiveOrchestration>();
            services.AddTransient<TransitiveActivity>();
            services.AddScoped<SimpleService>();
            services.AddScoped<DummyService>();
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
            
            orchestrationServiceAndClient.CreateIfNotExistsAsync().Wait();
        }

        public void RegisterOrchestrationAndActivities(TaskHubWorker taskHub, ServiceProvider serviceProvider)
        {
            taskHub.AddTaskOrchestrations(
                           typeof(TypedOrchestration),
                           typeof(MainOrchestration)
            );

            taskHub.AddTaskOrchestrations(
                new ScopedOrchestrationCreator<DummyOrchestration>(serviceProvider),
                new ScopedOrchestrationCreator<ScopedOrchestration>(serviceProvider),
                new ScopedOrchestrationCreator<TransitiveOrchestration>(serviceProvider)
            );


            taskHub.AddTaskActivities(typeof(TypedActivity));

            taskHub.AddTaskActivities(
                new ScopedActivityCreator<ScopedActivity>(serviceProvider),
                new ScopedActivityCreator<DummyActivity>(serviceProvider),
                new ScopedActivityCreator<TransitiveActivity>(serviceProvider)
            );
        }

    }
}
