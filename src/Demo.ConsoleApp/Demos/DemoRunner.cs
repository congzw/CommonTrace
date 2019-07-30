using System;
using System.Threading.Tasks;

namespace Demo.ConsoleApp.Demos
{
    public class DemoRunner
    {
        public static DemoRunner Instance = new DemoRunner();

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
