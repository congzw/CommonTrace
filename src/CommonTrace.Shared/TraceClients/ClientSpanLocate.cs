namespace CommonTrace.TraceClients
{
    public interface IClientTracerLocate
    {
        string TracerId { get; set; }
    }

    public interface IClientTraceLocate : IClientTracerLocate
    {
        string TraceId { get; set; }
    }

    public interface IClientSpanLocate : IClientTraceLocate
    {
        string SpanId { get; set; }
        string ParentSpanId { get; set; }
    }

    public enum ClientSpanLocateMode
    {
        ForCurrent = 0,
        ForParent = 1
    }
}