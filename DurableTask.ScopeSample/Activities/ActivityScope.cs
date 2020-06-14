using DurableTask.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample.Activities
{
    public class ActivityScope : TaskActivity, IDisposable
    {
        private readonly IServiceProvider services;
        private readonly Type activityType;
        internal ActivityScope(Type activityType, IServiceProvider services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));

            this.activityType = activityType ?? throw new ArgumentNullException(nameof(activityType));

            counter = ObjectsAnalyzer.GetOrCreateCounterFor<ActivityScope>();
            counter.Create();
        }
        ObjectsCounter counter;
        ~ActivityScope() => counter.Finalized();
        public void Dispose() => counter.Dispose();

        public override string Run(TaskContext context, string input)
        {
            using (IServiceScope scope = this.services.CreateScope())
            {
                var activity = (TaskActivity)scope.ServiceProvider.GetService(activityType);

                return activity.Run(context, input);
            }
        }
        
        public override Task<string> RunAsync(TaskContext context, string input)        
        {
            using (IServiceScope scope = this.services.CreateScope())
            {
                var activity = (TaskActivity)scope.ServiceProvider.GetService(activityType);

                return activity.RunAsync(context, input);
            }
        }
    }
    
}
