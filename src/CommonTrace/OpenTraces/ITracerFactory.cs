using System.Collections.Generic;
using CommonTrace.Common;
using OpenTracing;

namespace CommonTrace.OpenTraces
{
    public interface ICachedTracer
    {
        IDictionary<string, ITracer> CachedTracers { get; }
    }

    public interface ITracerFactory : ICachedTracer
    {
        TraceConfig Config { get; set; }
        ITracer CreateTracer(string tracerId);
    }

    public class TraceConfig : SimpleConfig
    {
        public TraceConfig()
        {
            DefaultTracerId = "Default-Tracer";
        }

        public string DefaultTracerId { get; set; }
    }

    public static class TracerFactoryExtensions
    {
        public static ITracer GetOrCreate(this ITracerFactory tracerFactory, string tracerId)
        {
            var defaultTracerId = TryFixTracerId(tracerFactory, tracerId);
            var theTracerId = string.IsNullOrWhiteSpace(tracerId) ? defaultTracerId : tracerId;
            if (!tracerFactory.CachedTracers.ContainsKey(theTracerId))
            {
                tracerFactory.CachedTracers[theTracerId] = tracerFactory.CreateTracer(theTracerId);
            }
            return tracerFactory.CachedTracers[theTracerId];
        }

        public static string TryFixTracerId(this ITracerFactory tracerFactory, string tracerId)
        {
            if (tracerFactory.Config != null && !string.IsNullOrWhiteSpace(tracerFactory.Config.DefaultTracerId))
            {
                return tracerFactory.Config.DefaultTracerId;
            }
            return "Default-Tracer";
        }
    }
}