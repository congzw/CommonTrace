﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonTrace.Common;

namespace CommonTrace.TraceClients.ApiProxy
{
    public class ClientTracerApiProxySmartWrapper : IClientTracerApiProxy
    {
        private readonly IClientTracerApiProxy _nullApiProxy = NullClientTracerApiProxy.Instance;

        public ClientTracerApiProxySmartWrapper(IClientTracerApiProxy apiProxy)
        {
            Proxy = apiProxy;
            GetDateNow = DateHelper.Instance.GetDateNow;
            //todo config
            CheckSmart = CheckIfNotOkAndExpired.Create(TimeSpan.FromSeconds(3));
        }

        public IClientTracerApiProxy Proxy { get; set; }

        public CheckIfNotOkAndExpired CheckSmart { get; set; }
        
        public Func<DateTime> GetDateNow { get; set; }

        public Task StartSpan(ClientSpan args)
        {
            var isOk = CheckApiStatusOkSmart();
            if (!isOk)
            {
                return _nullApiProxy.StartSpan(args);
            }
            return SafeInvokeTask(Proxy.StartSpan(args));
        }

        public Task Log(LogArgs args)
        {
            var isOk = CheckApiStatusOkSmart();
            if (!isOk)
            {
                return _nullApiProxy.Log(args);
            }
            return SafeInvokeTask(Proxy.Log(args));
        }

        public Task SetTags(SetTagArgs args)
        {
            var isOk = CheckApiStatusOkSmart();
            if (!isOk)
            {
                return _nullApiProxy.SetTags(args);
            }
            return SafeInvokeTask(Proxy.SetTags(args));
        }

        public Task FinishSpan(FinishSpanArgs args)
        {
            var isOk = CheckApiStatusOkSmart();
            if (!isOk)
            {
                return _nullApiProxy.FinishSpan(args);
            }
            return SafeInvokeTask(Proxy.FinishSpan(args));
        }

        public Task<DateTime> GetDate()
        {
            var isOk = CheckApiStatusOkSmart();
            if (!isOk)
            {
                return _nullApiProxy.GetDate();
            }

            var task = Proxy.GetDate();
            var failTask = task.ContinueWith(HandleApiTaskEx, TaskContinuationOptions.OnlyOnFaulted);
            var theTask = Task.WhenAny(task, failTask);
            if (theTask == failTask)
            {
                return _nullApiProxy.GetDate();
            }

            return task;
        }

        
        private bool CheckApiStatusOkSmart()
        {
            return CheckSmart.CheckIfNecessary(
                GetDateNow(), 
                () => AsyncHelper.RunSync(TryTestApiConnection));
        }
        
        private Task SafeInvokeTask(Task task)
        {
            var failTask = task.ContinueWith(HandleApiTaskEx, TaskContinuationOptions.OnlyOnFaulted);
            return Task.WhenAny(task, failTask);
        }

        private void HandleApiTaskEx(Task source)
        {
            CheckSmart.StatusOk = false;
            source.Exception?.Handle(ex =>
            {
                //todo log ex
                Trace.WriteLine("ApiTaskEx: " + ex.Message);
                return true;
            });
        }

        public Task<bool> TryTestApiConnection()
        {
            return Proxy.TryTestApiConnection();
        }


        #region for di extensions and simple use

        private static readonly ClientTracerApiProxySmartWrapper Instance = new ClientTracerApiProxySmartWrapper(
                SimpleIoc.Instance.Resolve<IClientTracerApiProxy>() ?? NullClientTracerApiProxy.Instance);

        public static Func<IClientTracerApiProxy> Resolve { get; set; } = () => Instance;

        #endregion
    }
}