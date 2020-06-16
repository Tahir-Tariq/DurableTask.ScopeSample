using DurableTask.Core;
using DurableTask.ScopeSample.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample.Orchestrations
{
    public class DummyActivity : AsyncTaskActivity<string, string>
    {
        private readonly DummyService service;

        public DummyActivity(DummyService service) => this.service = service ?? throw new ArgumentNullException(nameof(service));            

        protected override async Task<string> ExecuteAsync(TaskContext context, string input)
        {
            await Task.Delay(500);
            this.service.DoSomethingWithExternalResource();
            return "Completed";
        }
    }
}
