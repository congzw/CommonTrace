using System.Threading.Tasks;
using CommonTrace.OpenTraces;

namespace Demo.ConsoleApp.DemoTrace
{
    public class FooData
    {
        public string GetUserInfo(string username)
        {
            var tracer = TracerContext.GetCurrent();
            using (var scope = tracer.BuildSpan("FooData-GetUserInfo").StartActive(true))
            {
                //按需使用
                scope.Span.Log("a log from FooData");
                scope.Span.SetTag("username", username);

                Task.Delay(20).Wait();
                var result = $"some info of {username}";
                return result;
            }
        }
    }
}