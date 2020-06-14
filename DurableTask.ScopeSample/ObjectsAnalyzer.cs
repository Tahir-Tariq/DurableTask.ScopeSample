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

            foreach (var item in counterTable)
            {
                stringBuilder.AppendLine($"{item.Key.Name} => {item.Value.GetStatsString()}");
            }
            return stringBuilder.ToString();
        }
    }
}
