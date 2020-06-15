using DurableTask.Core;
using System;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample
{
    public class ScopedOrchestration : TaskOrchestration<string, string, string, string>, IDisposable
    {
        private Guid instanceId;

        public ScopedOrchestration()
        {
            instanceId = Guid.NewGuid();
            counter = ObjectsAnalyzer.GetOrCreateCounterFor<ScopedOrchestration>();
            counter.Create();
        }
        ObjectsCounter counter;
        ~ScopedOrchestration() => counter.Finalized();
        public void Dispose() => counter.Dispose();


        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        OrchestrationContext context;
        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            this.context = context;
            return $"{MyIdentity}[{await Utility.CallActivities(context, input)}]";
        }
        
        public override string GetStatus()
        {            
            return this.context.OrchestrationInstance.InstanceId;
        }
    }

}
