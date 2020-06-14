using DurableTask.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample.Orchestration
{
    public class OrchestrationScope : TaskOrchestration, IDisposable
    {
        private readonly IServiceProvider services;
        private readonly Type orchestrationType;
        internal OrchestrationScope(Type orchestrationType, IServiceProvider services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));

            this.orchestrationType = orchestrationType ?? throw new ArgumentNullException(nameof(orchestrationType));

            counter = ObjectsAnalyzer.GetOrCreateCounterFor<OrchestrationScope>();
            counter.Create();
        }
        ObjectsCounter counter;
        ~OrchestrationScope() => counter.Finalized();
        public void Dispose() => counter.Dispose();       

        public override Task<string> Execute(OrchestrationContext context, string input)
        {
            using (IServiceScope scope = this.services.CreateScope())
            {
                var orchestration = (TaskOrchestration)scope.ServiceProvider.GetService(orchestrationType);

                return orchestration.Execute(context, input);
            }
        }

        public override void RaiseEvent(OrchestrationContext context, string name, string input)
        {
            using (IServiceScope scope = this.services.CreateScope())
            {
                var orchestration = (TaskOrchestration)scope.ServiceProvider.GetService(orchestrationType);

                orchestration.RaiseEvent(context, name, input);
            }
        }

        public override string GetStatus()
        {
            using (IServiceScope scope = this.services.CreateScope())
            {
                var orchestration = (TaskOrchestration)scope.ServiceProvider.GetService(orchestrationType);

                return orchestration.GetStatus();
            }
        }
    }
   
}
