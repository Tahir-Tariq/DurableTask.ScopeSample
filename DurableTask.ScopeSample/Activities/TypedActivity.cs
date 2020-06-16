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

        protected override string Execute(DurableTask.Core.TaskContext context, string input)
        {
            return this.GetType().Name + instanceId;
        }
    }    
}
