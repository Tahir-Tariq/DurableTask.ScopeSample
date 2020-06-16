using DurableTask.Core;
using System;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample
{
    public class TypedOrchestration : TaskOrchestration<string, string>, IDisposable
    {
        private Guid instanceId;

        public TypedOrchestration()
        {
            instanceId = Guid.NewGuid();
            counter = ObjectsAnalyzer.GetOrCreateCounterFor<TypedOrchestration>();
            counter.Create();
        }
        ObjectsCounter counter;
        ~TypedOrchestration() => counter.Finalized();
        public void Dispose() => counter.Dispose();        

        public string MyIdentity => this.GetType().Name + instanceId;

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            var output = await context.ScheduleTask<string>(typeof(TypedActivity), input);

            return $"{MyIdentity}[{output}]";
        }
    }

}
