using System.Threading.Tasks;

namespace CommonTrace.TraceClients.Api
{
    public interface IClientTracerApi
    {
        Task StartSpan(ClientSpan args);
        Task Log(LogArgs args);
        Task SetTags(SetTagArgs args);
        Task FinishSpan(FinishSpanArgs args);
        
        ////todo for simple record spans only call once
        //Task SaveSpans(SaveSpansArgs args);
    }
}