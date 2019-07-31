using System;
using CommonTrace.Common;
using CommonTrace.TraceClients;
using CommonTrace.TraceClients.ApiProxy;

namespace Demo.ConsoleApp.DemoTraceClients
{
    public class FooClient
    {
        public void CallApi()
        {
            var clientTracerApiProxy = ApiProxyContext.Current;
            var isOk = AsyncHelper.RunSync(() => clientTracerApiProxy.TryTestApiConnection());
            Console.WriteLine(isOk);
        }
    }
}
