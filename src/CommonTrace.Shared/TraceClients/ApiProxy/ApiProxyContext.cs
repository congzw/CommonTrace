namespace CommonTrace.TraceClients.ApiProxy
{
    public class ApiProxyContext
    {
        public static IClientTracerApiProxy Current => ClientTracerApiProxySmartWrapper.Resolve();
    }
}