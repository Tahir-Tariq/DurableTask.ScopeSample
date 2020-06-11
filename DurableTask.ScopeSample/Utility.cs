using DurableTask.Core;
using System;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample
{
    public static class Utility
    {
        public static async Task<string> CallActivities(OrchestrationContext context, string input)
        {
            var output = await context.ScheduleTask<string>(typeof(Activity), input);

            output += await context.ScheduleTask<string>(typeof(ScopedActivity), input);

            output += await context.ScheduleTask<string>(typeof(TransitiveActivity), input);

            return output;
        }

        public static void WriteLine(string line)
         => Console.WriteLine(line);

         public static string FormatInstance(string name, Guid instance)
            => name + ": " + instance + " ";
    }

    
}
