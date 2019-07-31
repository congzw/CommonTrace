using System.Threading.Tasks;
using CommonTrace.OpenTraces;

namespace Demo.ConsoleApp.DemoTrace
{
    public class FooService
    {
        private readonly FooData _fooData;

        public FooService(FooData fooData)
        {
            _fooData = fooData;
        }
        
        public string GetUserInfo(string username)
        {
            var tracer = TracerContext.GetCurrent();
            using (var scope = tracer.BuildSpan("FooService-GetUserInfo").StartActive(true))
            {
                //按需使用
                scope.Span.Log("a log from FooService");
                scope.Span.SetTag("username", username);

                var result = _fooData.GetUserInfo(username);

                SomeWorkNeedTrace();
                SomeWorkNotNeedTrace();

                return result;
            }
        }

        public void SomeWorkNeedTrace()
        {
            //will use "FooService-GetUserInfo" span
            var tracer = TracerContext.GetCurrent();
            tracer.ActiveSpan.Log("another log from FooService's SomeWork");
            Task.Delay(100).Wait();
        }

        public void SomeWorkNotNeedTrace()
        {
            Task.Delay(50).Wait();
        }
    }
}
