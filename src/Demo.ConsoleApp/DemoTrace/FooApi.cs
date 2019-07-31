using CommonTrace.OpenTraces;

namespace Demo.ConsoleApp.DemoTrace
{
    public class FooApi
    {
        private readonly FooService _fooService;

        public FooApi(FooService fooService)
        {
            _fooService = fooService;
        }
        
        public string GetUserInfo(string username)
        {
            var tracer = TracerContext.GetCurrent();
            using (var scope = tracer.BuildSpan("FooApi-GetUserInfo").StartActive(true))
            {
                var result = _fooService.GetUserInfo(username);

                //按需使用
                scope.Span.Log("a log from FooApi");
                scope.Span.SetTag("username", username);

                return result;
            }
        }
    }
}