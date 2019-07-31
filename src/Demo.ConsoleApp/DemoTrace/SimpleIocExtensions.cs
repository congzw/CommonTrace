using CommonTrace.Common;
using CommonTrace.Jaeger;
using CommonTrace.OpenTraces;

namespace Demo.ConsoleApp.DemoTrace
{
    public static class SimpleIocExtensions
    {
        public static void InitDemoTrace(this SimpleIoc simpleIoc)
        {
            simpleIoc.Register<FooData>(() => new FooData());
            simpleIoc.Register<FooService>(() => new FooService(simpleIoc.Resolve<FooData>()));
            simpleIoc.Register<FooApi>(() => new FooApi(simpleIoc.Resolve<FooService>()));

            var traceConfig = new TraceConfig();
            traceConfig.DefaultTracerId = "Demo.ConsoleApp";
            traceConfig.SetTraceEndPoint("http://192.168.1.182:14268/api/traces");
            var tracerFactory = new JaegerTracerFactory(new MyLoggerFactory(), traceConfig);

            TracerContext.Resolve = () => new TracerContext(tracerFactory);
        }
    }
}
