using System;
using System.Threading.Tasks;
using CommonTrace.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTrace.TraceClients.ApiProxy
{
    [TestClass]
    public class ClientTracerApiProxySmartWrapperSpec
    {
        [TestMethod]
        public async Task StartSpan_StatusOk_Should_InvokeRealProxy()
        {
            var apiProxy = Create();
            apiProxy.ApiStatusOk = true;
            await apiProxy.StartSpan(null);
            ((MockClientTracerApiProxy)apiProxy.Proxy).StartSpanInvoked.ShouldTrue();
        }

        [TestMethod]
        public async Task StartSpan_StatusNotOk_Should_NotInvokeRealProxy()
        {
            var apiProxy = Create();
            apiProxy.ApiStatusOk = false;
            await apiProxy.StartSpan(null);
            ((MockClientTracerApiProxy)apiProxy.Proxy).StartSpanInvoked.ShouldFalse();
        }

        [TestMethod]
        public async Task StartSpan_StatusNotOk_IntervalNotExpired_Should_NotInvokeRealProxy()
        {
            var apiProxy = Create();
            apiProxy.ApiStatusOk = false;
            apiProxy.ExpiredIn.LastCheckAt = _mockNow.AddSeconds(-(checkIntervalSeconds -1));
            await apiProxy.StartSpan(null);
            ((MockClientTracerApiProxy)apiProxy.Proxy).StartSpanInvoked.ShouldFalse();
        }

        [TestMethod]
        public async Task StartSpan_StatusNotOk_IntervalExpired_Should_InvokeTryTestApiConnectionFail()
        {
            var apiProxy = Create();
            apiProxy.ExpiredIn.LastCheckAt = _mockNow.AddSeconds(-(checkIntervalSeconds + 1));
            await apiProxy.StartSpan(null);
            ((MockClientTracerApiProxy)apiProxy.Proxy).TryTestApiConnectionInvoked.ShouldTrue();
            ((MockClientTracerApiProxy)apiProxy.Proxy).StartSpanInvoked.ShouldFalse();
        }

        [TestMethod]
        public async Task StartSpan_StatusNotOk_IntervalExpired_Should_InvokeTryTestApiConnectionOk()
        {
            var apiProxy = Create(true);
            apiProxy.ExpiredIn.LastCheckAt = _mockNow.AddSeconds(-(checkIntervalSeconds + 1));
            await apiProxy.StartSpan(null);
            ((MockClientTracerApiProxy)apiProxy.Proxy).TryTestApiConnectionInvoked.ShouldTrue();
            ((MockClientTracerApiProxy)apiProxy.Proxy).StartSpanInvoked.ShouldTrue();
        }

        private readonly DateTime _mockNow = new DateTime(2019, 1, 1);
        private readonly int checkIntervalSeconds = 3;
        private ClientTracerApiProxySmartWrapper Create(bool statusOk = false)
        {
            var proxy = new MockClientTracerApiProxy();
            proxy.MockStatusIsOk = statusOk;
            var wrapper = new ClientTracerApiProxySmartWrapper(proxy);
            wrapper.ExpiredIn = ExpiredIn.Create(TimeSpan.FromSeconds(checkIntervalSeconds));
            wrapper.GetDateNow = () => _mockNow;
            return wrapper;
        }
    }

    public class MockClientTracerApiProxy : IClientTracerApiProxy
    {
        public bool StartSpanInvoked { get; set; }

        public Task StartSpan(ClientSpan args)
        {
            StartSpanInvoked = true;
            return Task.FromResult(0);
        }

        public Task Log(LogArgs args)
        {
            return Task.FromResult(0);
        }

        public Task SetTags(SetTagArgs args)
        {
            return Task.FromResult(0);
        }

        public Task FinishSpan(FinishSpanArgs args)
        {
            return Task.FromResult(0);
        }

        public Task<DateTime> GetDate()
        {
            return Task.FromResult(DateHelper.Instance.GetDateDefault());
        }

        public bool MockStatusIsOk { get; set; }
        public bool TryTestApiConnectionInvoked { get; set; }
        public Task<bool> TryTestApiConnection()
        {
            TryTestApiConnectionInvoked = true;
            return Task.FromResult(MockStatusIsOk);
        }
    }
}
