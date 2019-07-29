using System;
using System.Collections.Generic;

namespace CommonTrace.TraceClients
{
    public class ClientSpan : IClientSpanLocate
    {
        public ClientSpan()
        {
            Bags = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string TracerId { get; set; }
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }
        public string OpName { get; set; }

        //for extensions
        public IDictionary<string, string> Bags { get; set; }

        public static ClientSpan TryCreate(ClientSpanLocateMode mode, IClientSpanLocate locate, string opName, IDictionary<string, string> bags = null)
        {
            var shouldReturnNull = locate.IsBadLocateArgs(mode);
            if (shouldReturnNull)
            {
                return null;
            }

            var clientSpan = new ClientSpan();
            clientSpan.With(locate);
            clientSpan.OpName = opName;
            if (bags != null)
            {
                clientSpan.Bags = bags;
            }
            return clientSpan;
        }
        
        public static ClientSpan Create(IClientSpanLocate locate, string opName, IDictionary<string, string> bags = null)
        {
            if (locate == null)
            {
                throw new ArgumentNullException(nameof(locate));
            }
            return Create(locate.TracerId, locate.TraceId, locate.SpanId, locate.ParentSpanId, opName, bags);
        }

        public static ClientSpan Create(string tracerId, string traceId, string parentSpanId, string spanId, string opName, IDictionary<string, string> bags = null)
        {
            var clientSpan = new ClientSpan();
            if (bags != null)
            {
                clientSpan.Bags = bags;
            }

            //todo validate
            clientSpan.TracerId = tracerId;
            clientSpan.TraceId = traceId;
            clientSpan.SpanId = spanId;
            clientSpan.ParentSpanId = parentSpanId;
            clientSpan.OpName = opName;

            return clientSpan;
        }
    }

    public class LogArgs : IClientSpanLocate
    {
        public LogArgs()
        {
            Logs = new Dictionary<string, object>();
        }

        public string TracerId { get; set; }
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }

        public IDictionary<string, object> Logs { get; set; }

        public LogArgs WithLog(string key, object value)
        {
            //todo validate
            Logs[key] = value;
            return this;
        }
    }

    public class SetTagArgs : IClientSpanLocate
    {
        public SetTagArgs()
        {
            Tags = new Dictionary<string, object>();
        }

        public string TracerId { get; set; }
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }

        public IDictionary<string, object> Tags { get; set; }

        public SetTagArgs WithTag(string key, object value)
        {
            //todo validate
            Tags[key] = value;
            return this;
        }
    }

    public class FinishSpanArgs : IClientSpanLocate
    {
        public string TracerId { get; set; }
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }
    }
}
