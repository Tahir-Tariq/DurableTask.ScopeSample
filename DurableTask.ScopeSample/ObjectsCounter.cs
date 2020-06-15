using System.Threading;

namespace DurableTask.ScopeSample
{
    public class ObjectsCounter
    {
        public int created = 0;
        public int finalized = 0;
        public int disposed = 0;

        public void Create() => Interlocked.Increment(ref created);
        public void Dispose() => Interlocked.Increment(ref disposed);
        public void Finalized() => Interlocked.Increment(ref finalized);               
    }
}
