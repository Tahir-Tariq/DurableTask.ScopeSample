using System.Threading;

namespace DurableTask.ScopeSample
{
    public class ObjectsCounter
    {
        private int created = 0;
        private int finalized = 0;
        private int disposed = 0;

        public void Create() => Interlocked.Increment(ref created);
        public void Dispose() => Interlocked.Increment(ref disposed);
        public void Finalized() => Interlocked.Increment(ref finalized); 
       
        public string GetStatsString()
        {
            return string.Format("created: {0}, disposed: {1}, finalized: {2}", created, disposed, finalized);
        }
    }
}
