using DurableTask.Core;
using System;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample
{
    public class TransitiveOrchestration : TaskOrchestration<string, string>, IDisposable
    {
        private Guid instanceId;

        public TransitiveOrchestration()
        {
            instanceId = Guid.NewGuid();
            counter = ObjectsAnalyzer.GetOrCreateCounterFor<TransitiveOrchestration>();
            counter.Create();
        }
        ObjectsCounter counter;
        ~TransitiveOrchestration() => counter.Finalized();
        public void Dispose() => counter.Dispose();

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            return $"{MyIdentity}[{await Utility.CallActivities(context, input)}]";
        }        
    }

}
