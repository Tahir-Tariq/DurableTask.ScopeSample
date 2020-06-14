using System;

namespace DurableTask.ScopeSample
{
    public class SimpleService : IDisposable
    {
        public SimpleService()
        {
            counter = ObjectsAnalyzer.GetOrCreateCounterFor<SimpleService>();
            counter.Create();
        }
        ObjectsCounter counter;

        ~SimpleService() => counter.Finalized();
        public void Dispose() => counter.Dispose();

        public string Execute(object input)
        {            
            return string.Empty;
        }
    }    
}
