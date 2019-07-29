using System;
using OpenTracing;

namespace CommonTrace.OpenTraces
{
    public class TracerContext
    {
        public TracerContext(ITracerFactory tracerFactory)
        {
            Factory = tracerFactory ?? throw new ArgumentNullException(nameof(tracerFactory));
        }

        public ITracerFactory Factory { get; set; }

        public ITracer Current(string tracerId = null)
        {
            return Factory.GetOrCreate(tracerId);
        }

        #region for simple use

        public static ITracer GetCurrent(string tracerId = null)
        {
            var tracer = Resolve().Current(tracerId);
            return tracer;
        }

        #endregion

        #region for di extensions

        public static Func<TracerContext> Resolve { get; set; } = () => new TracerContext(NullTracerFactory.Instance);

        #endregion
    }
}