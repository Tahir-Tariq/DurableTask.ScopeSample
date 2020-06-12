using DurableTask.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DurableTask.ScopeSample
{
    public sealed class SimpleService: IDisposable, IMyIdentity
    {
        private Guid instanceId;
        public SimpleService()
        {
            instanceId = Guid.NewGuid();            
        }

        ~SimpleService() => Utility.WriteLine("Finalizer: " + MyIdentity);

        public void Dispose() => Utility.WriteLine("Destroying: " + MyIdentity);

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        public string Execute(object input)
        {
            string inputAsString = (string)input;

            return MyIdentity;
        }
    }

    public class ProxyRequest
    {
        public Type ServiceType { get; set; }
        public object ServiceInput { get; set; }
    }

    public sealed class ProxyActivity : TaskActivity<ProxyRequest, string>, IDisposable, IMyIdentity
    {
        private Guid instanceId;
        readonly IServiceProvider serviceProvider;
        public ProxyActivity(IServiceProvider serviceProvider)
        {
            instanceId = Guid.NewGuid();
            this.serviceProvider = serviceProvider;
        }

        ~ProxyActivity() => Utility.WriteLine("Finalizer: " + MyIdentity);

        public void Dispose() => Utility.WriteLine("Destroying: " + MyIdentity);

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        protected override string Execute(DurableTask.Core.TaskContext context, ProxyRequest input)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                SimpleService service = scope.ServiceProvider.GetService(input.ServiceType) as SimpleService;

                return $"{MyIdentity}[{service.Execute(input.ServiceInput)}]";
            }
        }
    }

    public sealed class TypedActivity : TaskActivity<string, string>, IDisposable, IMyIdentity
    {
        private Guid instanceId;
        public TypedActivity()
        {
            instanceId = Guid.NewGuid();
        }

        ~TypedActivity() => Utility.WriteLine("Finalizer: " + MyIdentity);

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
        readonly SimpleService service;
        public ScopedActivity(SimpleService service)
        {
            instanceId = Guid.NewGuid();
            this.service = service;
        }
        ~ScopedActivity() => Utility.WriteLine("Finalizer: " + MyIdentity);

        public void Dispose() => Utility.WriteLine("Destroying: " + MyIdentity);

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        protected override string Execute(DurableTask.Core.TaskContext context, string input)
        {
            return $"{MyIdentity}[{service.Execute(input)}]";
        }
    }

    public sealed class TransitiveActivity : TaskActivity<string, string>, IDisposable, IMyIdentity
    {
        private Guid instanceId;
        readonly SimpleService service;
        public TransitiveActivity(SimpleService service)
        {
            instanceId = Guid.NewGuid();
            this.service = service;
        }
        ~TransitiveActivity() => Utility.WriteLine("Finalizer: " + MyIdentity);

        public void Dispose() => Utility.WriteLine("Destroying: " + MyIdentity);

        public string MyIdentity => Utility.FormatInstance(this.GetType().Name, instanceId);

        protected override string Execute(DurableTask.Core.TaskContext context, string input)
        {
            return $"{MyIdentity}[{service.Execute(input)}]";
        }
    }
}
