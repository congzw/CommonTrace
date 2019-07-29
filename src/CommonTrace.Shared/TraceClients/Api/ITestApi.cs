using System;
using System.Threading.Tasks;

namespace CommonTrace.TraceClients.Api
{
    public interface ITestApi
    {
        Task<DateTime> GetDate();
    }
}