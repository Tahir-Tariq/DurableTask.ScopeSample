using DurableTask.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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

    public sealed class TransitiveActivity : TaskActivity<string, string>, IDisposable
    {
        private Guid instanceId;
        readonly SimpleService service;
        public TransitiveActivity(SimpleService service)
        {
            instanceId = Guid.NewGuid();
            this.service = service;
            counter = ObjectsAnalyzer.GetOrCreateCounterFor<TransitiveActivity>();
            counter.Create();
        }
        ObjectsCounter counter;
        ~TransitiveActivity() => counter.Finalized();
        public void Dispose() => counter.Dispose();

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        protected override string Execute(DurableTask.Core.TaskContext context, string input)
        {
            return $"{MyIdentity}[{service.Execute(input)}]";
        }
    }
}
