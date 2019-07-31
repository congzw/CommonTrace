using System;
using System.Threading.Tasks;
using CommonTrace.Common;

namespace CommonTrace.TraceClients.ApiProxy
{
    public class HttpClientTracerApiProxy : IClientTracerApiProxy
    {
        private readonly IWebApiHelper _webApiHelper;
        private readonly ApiProxyConfig _config;

        public HttpClientTracerApiProxy(IWebApiHelper webApiHelper, ApiProxyConfig apiProxyConfig)
        {
            _webApiHelper = webApiHelper;
            _config = apiProxyConfig;
        }

        public Task StartSpan(ClientSpan args)
        {
            //todo validate and throw ex
            var requestUri = _config.GetRequestUri(nameof(StartSpan));
            return _webApiHelper.PostAsJson(requestUri, args);
        }

        public Task Log(LogArgs args)
        {
            var requestUri = _config.GetRequestUri(nameof(Log));
            return _webApiHelper.PostAsJson(requestUri, args);
        }

        public Task SetTags(SetTagArgs args)
        {
            var requestUri = _config.GetRequestUri(nameof(SetTags));
            return _webApiHelper.PostAsJson(requestUri, args);
        }

        public Task FinishSpan(FinishSpanArgs args)
        {
            var requestUri = _config.GetRequestUri(nameof(FinishSpan));
            return _webApiHelper.PostAsJson(requestUri, args);
        }

        public Task<DateTime> GetDate()
        {
            var requestUri = _config.GetRequestUri(nameof(GetDate));
            return _webApiHelper.GetAsJson<DateTime>(requestUri, new DateTime(2000, 1, 1));
        }

        public Task<bool> TryTestApiConnection()
        {
            throw new NotImplementedException();
            //var requestUri = _config.GetRequestUri(nameof(GetDate));
            //return _webApiHelper.CheckTargetStatus(requestUri, );
        }
    }
}