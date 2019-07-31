using CommonTrace.OpenTraces;

namespace CommonTrace.Jaeger
{
    public static class TraceConfigExtensions
    {
        public static string TraceEndPointKey = "TraceEndPoint";

        public static string GetTraceEndPoint(this TraceConfig config, string defaultValue = "http://localhost:14268/api/traces")
        {
            return config.TryGet(TraceEndPointKey, defaultValue);
        }

        public static void SetTraceEndPoint(this TraceConfig config, string value)
        {
            config.AddOrUpdate(TraceEndPointKey, value);
        }
    }
}