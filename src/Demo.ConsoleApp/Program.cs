using System;
using System.Threading.Tasks;
using CommonTrace.Common;
using Demo.ConsoleApp.Demos;
using Demo.ConsoleApp.DemoTrace;
using Demo.ConsoleApp.DemoTraceClients;

namespace Demo.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var simpleIoc = SimpleIoc.Instance;
            simpleIoc.InitDemoTrace();

            var fooApi = simpleIoc.Resolve<FooApi>();
            fooApi.GetUserInfo("admin");
            Console.WriteLine("foo api demo complete.");

            //simpleIoc.InitDemoClients();
            //var fooClient = simpleIoc.Resolve<FooClient>();
            //fooClient.CallApi();
            //Console.WriteLine("foo client demo complete.");
            //Console.Read();

            //DemoRunner.Instance.TestHttpClient();
            //DemoRunner.Instance.TaskExBad().Wait();
            //DemoRunner.Instance.TaskEx(true).Wait();
            //DemoRunner.Instance.TaskEx(false).Wait();

            for (int i = 0; i < 10; i++)
            {
                Task.Delay(200).Wait();
                Console.Write(".");
            }
            Console.WriteLine("main method complete.");
            Console.Read();
        }

    }
}
