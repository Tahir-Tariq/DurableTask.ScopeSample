using DurableTask.Core;
using DurableTask.ScopeSample.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample.Orchestrations
{
    public class DummyOrchestration : TaskOrchestration<string, string>, IDisposable
    {
        public static int completed = 0;
        public static int disposed = 0;

        private readonly DummyService service;

        public DummyOrchestration()
        {
            this.service = new DummyService();
        }

        public override async Task<string> RunTask(OrchestrationContext context, string input)
        {
            try
            {
                await Task.Delay(5000);
                this.service.DoSomethingWithExternalResource();
                return "Completed";
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine("Service disposed prematurely!");
            }
            finally
            {
                Interlocked.Increment(ref completed);
            }

            return "Not completed";
        }

        public void Dispose()
        {
            Interlocked.Increment(ref disposed);
            this.service.Dispose();
        }
    }
}
