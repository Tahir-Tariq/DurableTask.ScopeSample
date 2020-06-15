using System;
using System.Collections.Concurrent;
using System.Text;

namespace DurableTask.ScopeSample
{
    public static class ObjectsAnalyzer
    {
        private static ConcurrentDictionary<Type, ObjectsCounter> counterTable = new ConcurrentDictionary<Type, ObjectsCounter>();

        public static ObjectsCounter GetOrCreateCounterFor<TObject>()
            => counterTable.GetOrAdd(typeof(TObject), (type) => new ObjectsCounter());         

        public static string GetStatsString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("|Type |Created| Disposed| Finalized|");
            stringBuilder.AppendLine("|--:|--|--|--|");

            foreach (var item in counterTable)
            {                
                stringBuilder.AppendLine($"|{item.Key.Name} {GetStatsString(item.Value)}");
            }
            return stringBuilder.ToString();
        }

        private static string GetStatsString(ObjectsCounter counter)
        {
            return string.Format("|{0}| {1}| {2}|", counter.created, counter.disposed, counter.finalized);
        }
    }
}
