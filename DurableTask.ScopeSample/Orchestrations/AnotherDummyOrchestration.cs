using DurableTask.Core;
using DurableTask.ScopeSample.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample.Orchestrations
{
    public class AnotherDummyOrchestration : TaskOrchestration<string, string>
    {        
        public override Task<string> RunTask(OrchestrationContext context, string input)
        {
            return context.ScheduleTask<string>(typeof(DummyActivity), input);
        }
    }
}
