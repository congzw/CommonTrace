using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommonTrace.TraceClients.Api
{
    public interface IClientTracerApi
    {
        Task StartSpan(ClientSpan args);
        Task Log(LogArgs args);
        Task SetTags(SetTagArgs args);
        Task FinishSpan(FinishSpanArgs args);

        ////save multi spans with only one call
        //Task SaveSpans(SaveSpans args);
    }

    public class SaveSpans
    {
        public SaveSpans()
        {
            Spans = new List<ClientSpanEntry>();
        }

        public IList<ClientSpanEntry> Spans { get; set; }
    }

    public class ClientSpanEntry : IClientSpanLocate
    {
        public string TracerId { get; set; }
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }
        public string OpName { get; set; }

        public IDictionary<string, object> Logs { get; set; }

        public ClientSpanEntry WithLog(string key, object value)
        {
            //todo validate
            Logs[key] = value;
            return this;
        }

        public IDictionary<string, object> Tags { get; set; }
        public ClientSpanEntry WithTag(string key, object value)
        {
            //todo validate
            Tags[key] = value;
            return this;
        }
    }
}