using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonTrace.Common;

namespace CommonTrace.TraceClients.ApiProxy
{
    public class ClientTracerApiProxySmartWrapper : IClientTracerApiProxy
    {
        private readonly IClientTracerApiProxy _nullApiProxy = new NullClientTracerApiProxy();

        public ClientTracerApiProxySmartWrapper(IClientTracerApiProxy apiProxy)
        {
            Proxy = apiProxy;
            GetDateNow = DateHelper.Instance.GetDateNow;
            //todo config
            CheckInterval = TimeSpan.FromSeconds(3);
        }

        public IClientTracerApiProxy Proxy { get; set; }

        public bool ApiStatusOk { get; set; }

        public TimeSpan CheckInterval { get; set; }

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
            if (!ApiStatusOk)
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

        public Task<bool> TryTestApiConnection()
        {
            return Proxy.TryTestApiConnection();
        }

        private DateTime? _lastCheckApiStatusDate = null;
        private bool CheckApiStatusOkSmart()
        {
            //check only necessary
            if (ApiStatusOk)
            {
                return true;
            }
            
            var now = GetDateNow();
            if (_lastCheckApiStatusDate == null || now - _lastCheckApiStatusDate <= CheckInterval)
            {
                return ApiStatusOk;
            }

            ApiStatusOk = TryTestApiConnection().Result;
            SetApiStatus(ApiStatusOk, now);

            return ApiStatusOk;
        }

        private Task SafeInvokeTask(Task task)
        {
            var failTask = task.ContinueWith(HandleApiTaskEx, TaskContinuationOptions.OnlyOnFaulted);
            return Task.WhenAny(task, failTask);
        }

        private void HandleApiTaskEx(Task source)
        {
            SetApiStatus(false, GetDateNow());
            source.Exception?.Handle(ex =>
            {
                //todo log ex
                Trace.WriteLine("ApiTaskEx: " + ex.Message);
                return true;
            });
        }

        public void SetApiStatus(bool apiStatusOk, DateTime checkDate)
        {
            ApiStatusOk = apiStatusOk;
            _lastCheckApiStatusDate = checkDate;
        }
    }
}