using DurableTask.AzureStorage;
using DurableTask.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DurableTask.ScopeSample
{
    public class DurableService
    {                
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

        public void RegisterOrchestrationAndActivities(TaskHubWorker taskHub)
        {
            taskHub.AddTaskOrchestrations(
                           typeof(TypedOrchestration),
                           typeof(MainOrchestration)
            );

            taskHub.AddTaskActivities(typeof(TypedActivity));
        }

    }
}
