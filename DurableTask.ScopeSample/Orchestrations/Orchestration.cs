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

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            var output = input + MyIdentity;

            output += await context.CreateSubOrchestrationInstance<string>(typeof(TransitiveOrchestration), input);

            output += await context.CreateSubOrchestrationInstance<string>(typeof(ScopedOrchestration), input);

            output += await context.CreateSubOrchestrationInstance<string>(typeof(TypedOrchestration), input);

            Interlocked.Increment(ref completedCount);

            return input;
        }
    }


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

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            return $"{MyIdentity}[{await Utility.CallActivities(context, input)}]";
        }
    }

    public class ScopedOrchestration : TaskOrchestration<string, string>, IDisposable
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

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            return $"{MyIdentity}[{await Utility.CallActivities(context, input)}]";
        }
    }

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
