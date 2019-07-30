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
        public async Task StartSpan_NotNeedCheck_Should_InvokeRealProxy()
        {
            var apiProxy = Create(false, true);
            await apiProxy.StartSpan(null);
            ((MockClientTracerApiProxy)apiProxy.Proxy).StartSpanInvoked.ShouldFalse();
        }

        [TestMethod]
        public async Task StartSpan_NeedCheck_Should_InvokeRealProxy()
        {
            var apiProxy = Create(true, true);
            await apiProxy.StartSpan(null);
            ((MockClientTracerApiProxy)apiProxy.Proxy).StartSpanInvoked.ShouldTrue();
        }

        [TestMethod]
        public async Task StartSpan_NeedCheck_Should_InvokeTryTestApiConnectionFail()
        {
            var apiProxy = Create(true, false);
            await apiProxy.StartSpan(null);
            ((MockClientTracerApiProxy)apiProxy.Proxy).TryTestApiConnectionInvoked.ShouldTrue();
            ((MockClientTracerApiProxy)apiProxy.Proxy).StartSpanInvoked.ShouldFalse();
        }

        [TestMethod]
        public async Task StartSpan_NeedCheck_Should_InvokeTryTestApiConnectionSuccess()
        {
            var apiProxy = Create(true, true);
            await apiProxy.StartSpan(null);
            ((MockClientTracerApiProxy)apiProxy.Proxy).TryTestApiConnectionInvoked.ShouldTrue();
            ((MockClientTracerApiProxy)apiProxy.Proxy).StartSpanInvoked.ShouldTrue();
        }
        
        private readonly DateTime _mockNow = new DateTime(2019, 1, 1);
        private ClientTracerApiProxySmartWrapper Create(bool needCheck, bool apiTestOkResult)
        {
            var proxy = new MockClientTracerApiProxy();
            proxy.MockApiTestOkResult = apiTestOkResult;
            var wrapper = new ClientTracerApiProxySmartWrapper(proxy);
            wrapper.GetDateNow = () => _mockNow;
            wrapper.CheckSmart = new MockCheckSmart(needCheck);
            return wrapper;
        }
    }

    public class MockCheckSmart : CheckIfNotOkAndExpired
    {
        private readonly bool _needCheck;

        public MockCheckSmart(bool needCheck)
        {
            _needCheck = needCheck;
        }
        
        public override bool CheckIfNecessary(DateTime now, Func<bool> checkStatusOkFunc)
        {
            if (_needCheck)
            {
                StatusOk = checkStatusOkFunc();
            }
            return StatusOk;
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

        public bool MockApiTestOkResult { get; set; }
        public bool TryTestApiConnectionInvoked { get; set; }
        public Task<bool> TryTestApiConnection()
        {
            TryTestApiConnectionInvoked = true;
            return Task.FromResult(MockApiTestOkResult);
        }
    }
}
