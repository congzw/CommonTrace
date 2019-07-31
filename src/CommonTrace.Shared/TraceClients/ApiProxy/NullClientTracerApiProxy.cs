using System;
using System.Threading.Tasks;
using CommonTrace.Common;

namespace CommonTrace.TraceClients.ApiProxy
{
    public class NullClientTracerApiProxy : IClientTracerApiProxy
    {
        public Task StartSpan(ClientSpan args)
        {
            return Task.FromResult(0);
        }

        public Task Log(LogArgs args)
        {
            return Task.FromResult(0);
        }

        public Task SetTags(SetTagArgs args)
        {
            return Task.FromResult(0);
        }

        public Task FinishSpan(FinishSpanArgs args)
        {
            return Task.FromResult(0);
        }

        public Task<DateTime> GetDate()
        {
            return Task.FromResult(DateHelper.Instance.GetDateDefault());
        }

        public Task<bool> TryTestApiConnection()
        {
            return Task.FromResult(false);
        }
    }
}