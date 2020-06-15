using DurableTask.Core;
using System;
using System.Threading.Tasks;

namespace DurableTask.ScopeSample
{
    public static class Utility
    {
        public static async Task<string> CallActivities(OrchestrationContext context, string input)
        {
            try
            {
                var output = await context.ScheduleTask<string>(typeof(TypedActivity), input);

                output += await context.ScheduleTask<string>(typeof(ScopedActivity), input);

                output += await context.ScheduleTask<string>(typeof(TransitiveActivity), input);               

                return output;
            }
            catch (Exception  ex)
            {

                return ex.Message;
            }
          
        }
     
         public static string FormatInstance(string name, Guid instance)
            => name + ": " + instance + " ";
    }

    
}
