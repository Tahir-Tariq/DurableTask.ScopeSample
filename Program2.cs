﻿//using DurableTask.AzureStorage;
//using DurableTask.Core;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Configuration;
//using System.Threading.Tasks;

//namespace DurableTask.ScopeSample
//{
//    class Program
//    {
//        public static string FormatInstance(string name, Guid instance)
//        => name + ": " + instance + " ";

//        static async Task Main(string[] args)
//        {
//            Console.WriteLine("Hello World!");

//            Program program = new Program();


//            TaskHubClient taskHubClient; TaskHubWorker taskHub;
//            program.Configure(out taskHubClient, out taskHub);

//            ServiceCollection collection = new ServiceCollection();
//            program.ConfigureServices(collection);
//            ServiceProvider serviceProvider = collection.BuildServiceProvider();

//            program.RegisterOrchestrationAndActivities(taskHub, serviceProvider);

//            await taskHub.StartAsync();
//            string request = "Request";

//            for (int i = 1; i <= 2; i++)
//            {
//                await taskHubClient.CreateOrchestrationInstanceAsync(typeof(MainOrchestration), request + i);
//            }
            

//            Console.WriteLine("Press enter to close");
//            Console.ReadLine();

//            Console.WriteLine("Memory used before collection:       {0:N0}",
//                       GC.GetTotalMemory(false));

//            // Collect all generations of memory.
//            GC.Collect();
//            Console.WriteLine("Memory used after full collection:   {0:N0}",
//                              GC.GetTotalMemory(true));

//            await taskHub.StopAsync();
//            Console.WriteLine("Task hub is stopped");
//            Console.ReadLine();
//        }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddScoped<ScopedOrchestration>();
//            services.AddScoped<ScopedActivity>();
//            services.AddTransient<TransitiveOrchestration>();
//            services.AddTransient<TransitiveActivity>();
//        }

//        public void Configure(out TaskHubClient taskHubClient, out TaskHubWorker taskHub)
//        {
//            string storageConnectionString = "UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://127.0.0.1:10002/";// ConfigurationManager.AppSettings["StorageConnectionString"];
//            string taskHubName = "ScopeSample";// ConfigurationManager.AppSettings["taskHubName"];

//            var orchestrationServiceAndClient =
//                new AzureStorageOrchestrationService(new AzureStorageOrchestrationServiceSettings
//                {
//                    StorageConnectionString = storageConnectionString,
//                    TaskHubName = taskHubName
//                });

//            taskHubClient = new TaskHubClient(orchestrationServiceAndClient);
//            taskHub = new TaskHubWorker(orchestrationServiceAndClient);

//            orchestrationServiceAndClient.CreateIfNotExistsAsync().Wait();
//        }

//        public void RegisterOrchestrationAndActivities(TaskHubWorker taskHub, ServiceProvider serviceProvider)
//        {
//            taskHub.AddTaskOrchestrations(
//                           typeof(TypedOrchestration),
//                           typeof(MainOrchestration)
//            );

//            taskHub.AddTaskOrchestrations(
//                ServiceProviderObjectCreator.CreateOrchestrationCreator<ScopedOrchestration>(serviceProvider),
//                ServiceProviderObjectCreator.CreateOrchestrationCreator<TransitiveOrchestration>(serviceProvider)
//            );

//            taskHub.AddTaskActivities(typeof(TypedActivity));


//            taskHub.AddTaskActivities(
//                ServiceProviderObjectCreator.CreateActivityCreator<ScopedActivity>(serviceProvider),
//                ServiceProviderObjectCreator.CreateActivityCreator<TransitiveActivity>(serviceProvider)
//            );
//        }

//    }

//    public class MainOrchestration : TaskOrchestration<string, string>, IDisposable
//    {
//        private Guid instanceId;

//        public MainOrchestration()
//        {
//            instanceId = Guid.NewGuid();
//        }

//        ~MainOrchestration() => Console.WriteLine("Finalizer: " + Program.FormatInstance(this.GetType().Name, instanceId));

//        public void Dispose()
//        {
//            Console.WriteLine("Destroying: " + Program.FormatInstance(this.GetType().Name, instanceId));
//        }

//        public override async Task<string> RunTask(OrchestrationContext context, string input)
//        {
//            var output = input + Program.FormatInstance(this.GetType().Name, instanceId);

//            output += await context.CreateSubOrchestrationInstance<string>(typeof(TransitiveOrchestration), input);

//            output += await context.CreateSubOrchestrationInstance<string>(typeof(ScopedOrchestration), input);

//            output += await context.CreateSubOrchestrationInstance<string>(typeof(TypedOrchestration), input);

//            Console.WriteLine(output);

//            return input;
//        }
//    }

//    public class TypedOrchestration : TaskOrchestration<string, string>, IDisposable
//    {
//        private Guid instanceId;

//        public TypedOrchestration()
//        {
//            instanceId = Guid.NewGuid();
//        }

//        ~TypedOrchestration() => Console.WriteLine("Finalizer: " + Program.FormatInstance(this.GetType().Name, instanceId));

//        public void Dispose()
//        {
//            Console.WriteLine("Destroying: " + Program.FormatInstance(this.GetType().Name, instanceId));
//        }

//        public override Task<string> RunTask(OrchestrationContext context, string input)
//        {
//            return Task.FromResult( Program.FormatInstance(this.GetType().Name, instanceId));
//        }
//    }

//    public class ScopedOrchestration : TaskOrchestration<string, string>, IDisposable
//    {
//        private Guid instanceId;

//        public ScopedOrchestration()
//        {
//            instanceId = Guid.NewGuid();
//        }

//        ~ScopedOrchestration() => Console.WriteLine("Finalizer: " + Program.FormatInstance(this.GetType().Name, instanceId));

//        public void Dispose()
//        {
//            Console.WriteLine("Destroying: " + Program.FormatInstance(this.GetType().Name, instanceId));
//        }

//        public override Task<string> RunTask(OrchestrationContext context, string input)
//        {
//            return Task.FromResult(Program.FormatInstance(this.GetType().Name, instanceId));
//        }
//    }

//    public class TransitiveOrchestration : TaskOrchestration<string, string>, IDisposable
//    {
//        private Guid instanceId;

//        public TransitiveOrchestration()
//        {
//            instanceId = Guid.NewGuid();
//        }

//        ~TransitiveOrchestration()=> Console.WriteLine("Finalizer: " + Program.FormatInstance(this.GetType().Name, instanceId));

//        public void Dispose()
//        {
//            Console.WriteLine("Destroying: " + Program.FormatInstance(this.GetType().Name, instanceId));
//        }

//        public override Task<string> RunTask(OrchestrationContext context, string input)
//        {
//            return Task.FromResult(Program.FormatInstance(this.GetType().Name, instanceId));
//        }
//    }

//    public sealed class TypedActivity : TaskActivity<string, string>
//    {
//        private Guid instanceId;
//        public TypedActivity()
//        {
//            instanceId = Guid.NewGuid();
//        }

//        protected override string Execute(DurableTask.Core.TaskContext context, string input)
//        {
//            return input += Program.FormatInstance(this.GetType().Name, instanceId);
//        }
//    }

//    public sealed class ScopedActivity : TaskActivity<string, string>
//    {
//        private Guid instanceId;
//        public ScopedActivity()
//        {
//            instanceId = Guid.NewGuid();
//        }

//        protected override string Execute(DurableTask.Core.TaskContext context, string input)
//        {
//            return Program.FormatInstance(this.GetType().Name, instanceId);
//        }
//    }

//    public sealed class TransitiveActivity : TaskActivity<string, string>
//    {
//        private Guid instanceId;
//        public TransitiveActivity()
//        {
//            instanceId = Guid.NewGuid();
//        }

//        protected override string Execute(DurableTask.Core.TaskContext context, string input)
//        {
//            return Program.FormatInstance(this.GetType().Name, instanceId);
//        }
//    }



//    public class ServiceProviderObjectCreator<T> : ObjectCreator<T>
//    {
//        readonly Type prototype;
//        readonly IServiceProvider serviceProvider;

//        public ServiceProviderObjectCreator(Type type, IServiceProvider serviceProvider)
//        {
//            this.prototype = type;
//            this.serviceProvider = serviceProvider;
//            Initialize(type);
//        }

//        public override T Create()
//        {
//            return (T)serviceProvider.GetService(this.prototype);
//        }

//        void Initialize(object obj)
//        {
//            Name = NameVersionHelper.GetDefaultName(obj);
//            Version = NameVersionHelper.GetDefaultVersion(obj);
//        }
//    }

//    public static class ServiceProviderObjectCreator
//    {
//        public static ServiceProviderObjectCreator<T> Create<T, TImpl>(IServiceProvider services) where TImpl : T
//            => new ServiceProviderObjectCreator<T>(typeof(TImpl), services);

//        public static ServiceProviderObjectCreator<TaskOrchestration> CreateOrchestrationCreator<TImpl>(IServiceProvider services) where TImpl : TaskOrchestration
//            => Create<TaskOrchestration, TImpl>(services);

//        public static ServiceProviderObjectCreator<TaskActivity> CreateActivityCreator<TImpl>(IServiceProvider services) where TImpl : TaskActivity
//            => Create<TaskActivity, TImpl>(services);
//    }
//}
