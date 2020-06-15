using DurableTask.Core;
using System;

namespace DurableTask.ScopeSample
{
    public sealed class ScopedActivity : TaskActivity<string, string>, IDisposable
    {
        private Guid instanceId;
        readonly SimpleService service;
        public ScopedActivity(SimpleService service)
        {
            instanceId = Guid.NewGuid();
            this.service = service;
            counter = ObjectsAnalyzer.GetOrCreateCounterFor<ScopedActivity>();
            counter.Create();
        }
        ObjectsCounter counter;
        ~ScopedActivity() => counter.Finalized();
        public void Dispose() => counter.Dispose();

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        protected override string Execute(DurableTask.Core.TaskContext context, string input)
        {
            return $"{MyIdentity}[{service.Execute(input)}]";
        }
    }
}
