using DurableTask.Core;
using DurableTask.ScopeSample.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample.Orchestrations
{
    public class DummyOrchestration : TaskOrchestration<string, string>
    {
        private readonly DummyService service;

        public DummyOrchestration(DummyService service) => this.service = service ?? throw new ArgumentNullException(nameof(service));

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            await Task.Delay(5000);
            this.service.DoSomethingWithExternalResource();
            return "Completed";
        }
    }
}
