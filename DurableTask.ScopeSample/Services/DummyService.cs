using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace DurableTask.ScopeSample.Services
{
    public class DummyService : IDisposable
    {
        private bool isDisposed = false;

        public void DoSomethingWithExternalResource()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(nameof(DummyService));
            }
        }

        public void Dispose() => this.isDisposed = true;
    }
}
