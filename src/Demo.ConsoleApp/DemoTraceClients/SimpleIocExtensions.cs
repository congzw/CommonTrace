using CommonTrace.Common;
using CommonTrace.TraceClients.ApiProxy;

namespace Demo.ConsoleApp.DemoTraceClients
{
    public static class SimpleIocExtensions
    {
        public static void InitDemoClients(this SimpleIoc simpleIoc)
        {
            simpleIoc.Register<FooClient>(() => new FooClient());

            //simpleIoc.Register<IClientTracerApiProxy>(() => new FooClient());
        }
    }
}
