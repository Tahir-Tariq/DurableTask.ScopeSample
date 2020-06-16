using DurableTask.Core;
using DurableTask.ScopeSample.Orchestrations;
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

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            string output = "";
            
            output += await context.CreateSubOrchestrationInstance<string>(typeof(DummyOrchestration), input);

            output += await context.CreateSubOrchestrationInstance<string>(typeof(AnotherDummyOrchestration), input);

            // Uncomment following line to see how Durable framework will behave when Orchestration has async call in it.
            //output += await context.CreateSubOrchestrationInstance<string>(typeof(TypedOrchestration), input);

            Interlocked.Increment(ref completedCount);

            return output;
        }
    }

}
