using System.Collections.Generic;
using OpenTracing;

namespace CommonTrace.OpenTraces
{
    public interface ICachedTracer
    {
        IDictionary<string, ITracer> CachedTracers { get; }
    }

    public interface ITracerFactory : ICachedTracer
    {
        ITracer CreateTracer(string tracerId);
    }

    public static class TracerFactoryExtensions
    {
        private static string DefaultTracerId = "Default-Tracer";
        public static ITracer GetOrCreate(this ITracerFactory tracerFactory, string tracerId)
        {
            var theTracerId = string.IsNullOrWhiteSpace(tracerId) ? DefaultTracerId : tracerId;
            if (!tracerFactory.CachedTracers.ContainsKey(theTracerId))
            {
                tracerFactory.CachedTracers[theTracerId] = tracerFactory.CreateTracer(theTracerId);
            }
            return tracerFactory.CachedTracers[theTracerId];
        }
    }
}