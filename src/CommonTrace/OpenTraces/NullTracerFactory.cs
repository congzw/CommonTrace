using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OpenTracing;
using OpenTracing.Mock;

namespace CommonTrace.OpenTraces
{
    public class NullTracerFactory : ITracerFactory
    {
        public NullTracerFactory()
        {
            CachedTracers = new ConcurrentDictionary<string, ITracer>(StringComparer.OrdinalIgnoreCase);
        }
        
        public ITracer CreateTracer(string tracerId)
        {
            return new MockTracer();
        }

        public IDictionary<string, ITracer> CachedTracers { get; }

        public static ITracerFactory Instance = new NullTracerFactory();
    }
}