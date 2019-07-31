using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTrace.TraceClients.ApiProxy
{
    [TestClass]
    public class ApiProxyContextSpec
    {
        [TestMethod]
        public void Current_NotSetupIoc_Should_Return_Default()
        {
            var clientTracerApiProxy = ApiProxyContext.Current;
            clientTracerApiProxy.ShouldNotNull();
            clientTracerApiProxy.GetType().Name.ShouldEqual(typeof(ClientTracerApiProxySmartWrapper).Name);
        }
    }
}
