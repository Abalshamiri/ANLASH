using System;
using Abp.Dependency;
using Abp.Events.Bus.Exceptions;
using Abp.Events.Bus.Handlers;
using Castle.Core.Logging;

namespace ANLASH.Debugging
{
    /// <summary>
    /// Global exception logger to catch all unhandled exceptions
    /// </summary>
    public class GlobalExceptionLogger : IEventHandler<AbpHandledExceptionData>, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public GlobalExceptionLogger()
        {
            Logger = NullLogger.Instance;
        }

        public void HandleEvent(AbpHandledExceptionData eventData)
        {
            Console.WriteLine("=".PadRight(80, '='));
            Console.WriteLine("GLOBAL EXCEPTION CAUGHT");
            Console.WriteLine("=".PadRight(80, '='));
            Console.WriteLine($"Exception Type: {eventData.Exception.GetType().Name}");
            Console.WriteLine($"Message: {eventData.Exception.Message}");
            Console.WriteLine($"Source: {eventData.Exception.Source}");
            Console.WriteLine($"StackTrace: {eventData.Exception.StackTrace}");
            
            if (eventData.Exception.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {eventData.Exception.InnerException.Message}");
                Console.WriteLine($"Inner StackTrace: {eventData.Exception.InnerException.StackTrace}");
            }
            
            Console.WriteLine("=".PadRight(80, '='));
            
            Logger.Error("Global exception caught", eventData.Exception);
        }
    }
}
