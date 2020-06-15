using DurableTask.Core;
using System;

namespace DurableTask.ScopeSample
{
    public sealed class TypedActivity : TaskActivity<string, string>, IDisposable
    {
        private Guid instanceId;
        public TypedActivity()
        {
            instanceId = Guid.NewGuid();
            counter = ObjectsAnalyzer.GetOrCreateCounterFor<TypedActivity>();
            counter.Create();
        }
        ObjectsCounter counter;
        ~TypedActivity() => counter.Finalized();
        public void Dispose() => counter.Dispose();

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        protected override string Execute(DurableTask.Core.TaskContext context, string input)
        {
            return MyIdentity;
        }
    }    
}
