using CommonTrace.Common;

namespace CommonTrace.TraceClients.ApiProxy
{
    public class ApiProxyConfig
    {
        public ApiProxyConfig()
        {
            BaseUri = "http://localhost:5000/api/trace";
        }

        public string BaseUri { get; set; }

        private bool fixBaseUri = false;

        public string GetRequestUri(string method)
        {
            if (!fixBaseUri)
            {
                BaseUri = BaseUri.TrimEnd('/') + "/";
            }

            return BaseUri + method;
        }
    }

    public static class ApiProxyConfigExtensions
    {
        public static ApiProxyConfig GetApiProxyConfig(this SimpleConfig simpleConfig)
        {
            return simpleConfig.TryGetModel<ApiProxyConfig>(null);
        }

        public static void SetApiProxyConfig(this SimpleConfig simpleConfig, ApiProxyConfig apiProxyConfig)
        {
            simpleConfig.AddOrUpdateModel(apiProxyConfig);
        }
    }
}
