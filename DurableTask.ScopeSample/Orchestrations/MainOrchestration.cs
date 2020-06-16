using DurableTask.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample
{

    public class MainOrchestration : TaskOrchestration<string, string>, IDisposable
    {
        private Guid instanceId;
        public static int completedCount;
        public MainOrchestration()
        {
            instanceId = Guid.NewGuid();
            counter = ObjectsAnalyzer.GetOrCreateCounterFor<MainOrchestration>();
            counter.Create();
        }
        ObjectsCounter counter;
        ~MainOrchestration() => counter.Finalized();
        public void Dispose() => counter.Dispose();
        

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            var output = input + this.GetType().Name + instanceId; ;
            
            output += await context.CreateSubOrchestrationInstance<string>(typeof(TypedOrchestration), input);            

            Interlocked.Increment(ref completedCount);

            return output;
        }
    }

}
