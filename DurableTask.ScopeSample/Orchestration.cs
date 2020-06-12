using DurableTask.Core;
using System;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample
{

    public class MainOrchestration : TaskOrchestration<string, string>, IDisposable, IMyIdentity
    {
        private Guid instanceId;

        public MainOrchestration()
        {
            instanceId = Guid.NewGuid();
        }

        ~MainOrchestration() => Utility.WriteLine("Finalizer: " + MyIdentity);
        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);
        public void Dispose()
        {
            Utility.WriteLine("Destroying: " + MyIdentity);
        }

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            var output = input + MyIdentity;

            output += await context.CreateSubOrchestrationInstance<string>(typeof(TransitiveOrchestration), input);

            output += await context.CreateSubOrchestrationInstance<string>(typeof(ScopedOrchestration), input);

            output += await context.CreateSubOrchestrationInstance<string>(typeof(TypedOrchestration), input);

            Utility.WriteLine(output);

            return input;
        }
    }


    public class TypedOrchestration : TaskOrchestration<string, string>, IDisposable, IMyIdentity
    {
        private Guid instanceId;

        public TypedOrchestration()
        {
            instanceId = Guid.NewGuid();
        }

        ~TypedOrchestration() => Utility.WriteLine("Finalizer: " + MyIdentity);


        public void Dispose()
        {
            Utility.WriteLine("Destroying: " + MyIdentity);
        }

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            return $"{MyIdentity}[{await Utility.CallActivities(context, input)}]";
        }
    }

    public class ScopedOrchestration : TaskOrchestration<string, string>, IDisposable, IMyIdentity
    {
        private Guid instanceId;

        public ScopedOrchestration()
        {
            instanceId = Guid.NewGuid();
        }

        ~ScopedOrchestration() => Utility.WriteLine("Finalizer: " + MyIdentity);

        public void Dispose()
        {
            Utility.WriteLine("Destroying: " + MyIdentity);
        }

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            return $"{MyIdentity}[{await Utility.CallActivities(context, input)}]";
        }
    }

    public class TransitiveOrchestration : TaskOrchestration<string, string>, IDisposable, IMyIdentity
    {
        private Guid instanceId;

        public TransitiveOrchestration()
        {
            instanceId = Guid.NewGuid();
        }

        ~TransitiveOrchestration() => Utility.WriteLine("Finalizer: " + MyIdentity);

        public void Dispose() => Utility.WriteLine("Destroying: " + MyIdentity);

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            return $"{MyIdentity}[{await Utility.CallActivities(context, input)}]";
        }
    }

}
