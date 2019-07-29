using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTrace.TraceClients.ApiProxy
{
    [TestClass]
    public class ApiProxyContextSpec
    {
        [TestMethod]
        public void GetClientTracerApiProxy_NotSetupIoc_Should_Return_Default()
        {
            var apiProxyContext = CreateApiProxyContext();
            var clientTracerApiProxy = apiProxyContext.GetClientTracerApiProxy();
            clientTracerApiProxy.ShouldNotNull();
            clientTracerApiProxy.GetType().Name.ShouldEqual(typeof(NullClientTracerApiProxy).Name);
        }

        private ApiProxyContext CreateApiProxyContext()
        {
            return ApiProxyContext.Resolve();
        }
    }
}
