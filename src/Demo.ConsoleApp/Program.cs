using System;
using System.Threading.Tasks;
using Demo.ConsoleApp.Demos;

namespace Demo.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //DemoRunner.Instance.TaskExBad().Wait();
            DemoRunner.Instance.TaskEx(true).Wait();
            DemoRunner.Instance.TaskEx(false).Wait();
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
