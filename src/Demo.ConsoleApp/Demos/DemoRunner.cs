using System;
using System.Threading.Tasks;
using CommonTrace.Common;

namespace Demo.ConsoleApp.Demos
{
    public class DemoRunner
    {
        public static DemoRunner Instance = new DemoRunner();

        public void TestHttpClient()
        {
            try
            {
                var webApiHelper = (WebApiHelper)WebApiHelper.Resolve();
                TestHttpClient(webApiHelper, "http://localhost:16685/api/test/GetDate", 50, 50);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        private void TestHttpClient(WebApiHelper webApiHelper, string testUri, int testCount, int failMs)
        {
            for (int i = 0; i < testCount; i++)
            {
                var i1 = i;
                Task.Run(async () =>
                {
                    var isOk = await webApiHelper.CheckTargetStatus(testUri, failMs);
                    Console.WriteLine("test " + i1 + " => " + (isOk ? "OK" : "!!!"));
                });
            }
        }

        public Task TaskExBad()
        {
            var task1 = Task.Run(() => throw new Exception("task1 faulted."));
            task1.ContinueWith(t => Console.WriteLine("ex!!!"), TaskContinuationOptions.OnlyOnFaulted);
            task1.ContinueWith(t => { /* on success */ }, TaskContinuationOptions.OnlyOnRanToCompletion);

            return task1;
        }

        public Task TaskEx(bool fail)
        {
            var task = Task.Run(() =>
            {
                if (fail)
                {
                    throw new Exception("task run faulted.");
                }
                else
                {
                    Console.WriteLine("task run success");
                }
            });
            var failTask = task.ContinueWith(LogIfErrors, TaskContinuationOptions.OnlyOnFaulted);
            var resultTask = Task.WhenAny(failTask, task);
            return resultTask;
        }

        public async Task AsyncTaskEx()
        {
            try
            {
                //var task = Task.Factory.StartNew(() => throw new Exception("task1 faulted.")).ConfigureAwait(false);
                var task = Task.Run(() => throw new Exception("task1 faulted.")).ConfigureAwait(false);
                await task;
            }
            catch (Exception e)
            {
                // Perform cleanup here.
                Console.WriteLine("Ex: " + e.Message);
            }
        }
        
        private static void LogIfErrors(Task source)
        {
            if (source.Exception == null)
            {
                return;
            }
            source.Exception.Handle(ex =>
            {
                Console.WriteLine("#unhandled task error: {0}", ex.Message);
                return true;
            });
        }
    }
}
