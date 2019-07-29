using System;
using CommonTrace.Common;

namespace CommonTrace.TraceClients.ApiProxy
{
    public class ApiProxyContext
    {
        public ApiProxyContext(SimpleIoc simpleIoc)
        {
            Factory = simpleIoc ?? throw new ArgumentNullException(nameof(simpleIoc));
            if (Factory.Resolve<IClientTracerApiProxy>() == null)
            {
                var nullProxy = new NullClientTracerApiProxy();
                Factory.Register<IClientTracerApiProxy>(() => nullProxy);
            }
        }

        public SimpleIoc Factory { get; set; }

        public IClientTracerApiProxy GetClientTracerApiProxy()
        {
            var clientTracerApiProxy = Factory.Resolve<IClientTracerApiProxy>() ?? new NullClientTracerApiProxy();
            return clientTracerApiProxy;
        }

        #region for simple use

        public static IClientTracerApiProxy ClientTracerApiProxy => Resolve().GetClientTracerApiProxy();

        #endregion

        #region for di extensions

        public static Func<ApiProxyContext> Resolve { get; set; } = () => new ApiProxyContext(SimpleIoc.Instance);

        #endregion
    }
}