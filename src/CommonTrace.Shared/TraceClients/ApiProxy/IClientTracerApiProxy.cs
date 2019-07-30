﻿using System;
using System.Threading.Tasks;
using CommonTrace.TraceClients.Api;

namespace CommonTrace.TraceClients.ApiProxy
{
    public interface IClientTracerApiProxy : IClientTracerApi, ITestApi
    {
        Task<bool> TryTestApiConnectionAsync();
        bool TryTestApiConnection();
    }
}