using DurableTask.Core;
using DurableTask.ScopeSample.Activities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DurableTask.ScopeSample
{
    internal class ScopedActivityCreator<T> : ObjectCreator<TaskActivity> where T : TaskActivity
    {
        readonly IServiceProvider serviceProvider;       

        public ScopedActivityCreator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.Name = NameVersionHelper.GetDefaultName(typeof(T));
            this.Version = NameVersionHelper.GetDefaultVersion(typeof(T));
        }

        public override TaskActivity Create() => new ActivityScope(typeof(T), serviceProvider);
    }
}