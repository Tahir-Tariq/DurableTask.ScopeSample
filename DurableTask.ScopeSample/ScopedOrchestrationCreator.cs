using DurableTask.Core;
using DurableTask.ScopeSample.Orchestration;
using System;

namespace DurableTask.ScopeSample
{
    internal class ScopedOrchestrationCreator<T> : ObjectCreator<TaskOrchestration> where T : TaskOrchestration
    {
        readonly IServiceProvider serviceProvider;

        public ScopedOrchestrationCreator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.Name = NameVersionHelper.GetDefaultName(typeof(T));
            this.Version = NameVersionHelper.GetDefaultVersion(typeof(T));
        }

        public override TaskOrchestration Create() => new OrchestrationScope(typeof(T), serviceProvider);
    }
}