using DurableTask.Core;
using System;

namespace DurableTask.ScopeSample
{
    public sealed class Activity : TaskActivity<string, string>, IDisposable, IMyIdentity
    {
        private Guid instanceId;
        public Activity()
        {
            instanceId = Guid.NewGuid();
        }

        ~Activity() => Utility.WriteLine("Finalizer: " + MyIdentity);

        public void Dispose() => Utility.WriteLine("Destroying: " + MyIdentity);

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        protected override string Execute(DurableTask.Core.TaskContext context, string input)
        {
            return MyIdentity;
        }
    }

    public sealed class ScopedActivity : TaskActivity<string, string>, IDisposable, IMyIdentity
    {
        private Guid instanceId;
        public ScopedActivity()
        {
            instanceId = Guid.NewGuid();
        }
        ~ScopedActivity() => Utility.WriteLine("Finalizer: " + MyIdentity);

        public void Dispose() => Utility.WriteLine("Destroying: " + MyIdentity);

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        protected override string Execute(DurableTask.Core.TaskContext context, string input)
        {
            return MyIdentity;
        }
    }

    public sealed class TransitiveActivity : TaskActivity<string, string>, IDisposable, IMyIdentity
    {
        private Guid instanceId;
        public TransitiveActivity()
        {
            instanceId = Guid.NewGuid();
        }
        ~TransitiveActivity() => Utility.WriteLine("Finalizer: " + MyIdentity);

        public void Dispose() => Utility.WriteLine("Destroying: " + MyIdentity);

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        protected override string Execute(DurableTask.Core.TaskContext context, string input)
        {
            return MyIdentity;
        }
    }
}
