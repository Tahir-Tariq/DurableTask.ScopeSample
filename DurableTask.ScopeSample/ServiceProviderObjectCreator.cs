using DurableTask.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DurableTask.ScopeSample
{
    public class ServiceProviderObjectCreator<T> : ObjectCreator<T>
    {
        readonly Type prototype;
        readonly IServiceProvider serviceProvider;

        public ServiceProviderObjectCreator(Type type, IServiceProvider serviceProvider)
        {
            this.prototype = type;
            this.serviceProvider = serviceProvider;
            Initialize(type);
        }

        public override T Create()
        {            
            return (T)serviceProvider.CreateScope().ServiceProvider.GetService(this.prototype);
        }

        void Initialize(object obj)
        {
            Name = NameVersionHelper.GetDefaultName(obj);
            Version = NameVersionHelper.GetDefaultVersion(obj);
        }
    }

    public static class ServiceProviderObjectCreator
    {
        public static ServiceProviderObjectCreator<T> Create<T, TImpl>(IServiceProvider services) where TImpl : T
            => new ServiceProviderObjectCreator<T>(typeof(TImpl), services);

        public static ServiceProviderObjectCreator<TaskOrchestration> CreateOrchestrationCreator<TImpl>(IServiceProvider services) where TImpl : TaskOrchestration
            => Create<TaskOrchestration, TImpl>(services);

        public static ServiceProviderObjectCreator<TaskActivity> CreateActivityCreator<TImpl>(IServiceProvider services) where TImpl : TaskActivity
            => Create<TaskActivity, TImpl>(services);
    }
}
