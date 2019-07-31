using System;
using CommonTrace.Common;
using Demo.ConsoleApp.DemoTrace;

namespace Demo.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var simpleIoc = SimpleIoc.Instance;
            SimpleIocInit.Init(simpleIoc);

            var fooApi = simpleIoc.Resolve<FooApi>();
            fooApi.GetUserInfo("admin");
            Console.WriteLine("foo api demo complete.");
            Console.Read();

            //DemoRunner.Instance.TaskExBad().Wait();
            //DemoRunner.Instance.TaskEx(true).Wait();
            //DemoRunner.Instance.TaskEx(false).Wait();
            //for (int i = 0; i < 10; i++)
            //{
            //    Task.Delay(200).Wait();
            //    Console.Write(".");
            //}
            //Console.WriteLine("main method complete.");
            //Console.Read();
        }

    }
}
