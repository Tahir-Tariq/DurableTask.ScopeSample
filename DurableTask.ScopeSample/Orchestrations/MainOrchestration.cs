﻿using DurableTask.Core;
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

            Task<string> scopedOrchestrationTask = context.CreateSubOrchestrationInstance<string>(typeof(ScopedOrchestration), context.OrchestrationInstance.ExecutionId, input);

            output += await context.CreateSubOrchestrationInstance<string>(typeof(TransitiveOrchestration), input);
            
            output += await context.CreateSubOrchestrationInstance<string>(typeof(TypedOrchestration), input);

            context.SendEvent(new OrchestrationInstance { InstanceId = context.OrchestrationInstance.ExecutionId }, "EventTest", "EventData");

            output += await scopedOrchestrationTask;

            Interlocked.Increment(ref completedCount);

            return input;
        }
    }

}
