namespace CommonTrace.TraceClients
{
    public static class ClientSpanLocateExtensions
    {
        public static string ToLocateCurrentKey(this IClientSpanLocate locate)
        {
            return ClientSpanLocateKeyHelper.ToLocateCurrentKey(locate);
        }

        public static string ToLocateParentKey(this IClientSpanLocate locate)
        {
            return ClientSpanLocateKeyHelper.ToLocateParentKey(locate);
        }

        public static string ToDisplayKey(this IClientSpanLocate locate)
        {
            return ClientSpanLocateKeyHelper.ToDisplayKey(locate);
        }

        public static T With<T>(this T locate, IClientSpanLocate setter) where T : IClientSpanLocate
        {
            return ClientSpanLocateKeyHelper.With(locate, setter);
        }

        public static T With<T>(this T locate, string tracerId, string traceId, string parentSpanId, string spanId) where T : IClientSpanLocate
        {
            return ClientSpanLocateKeyHelper.With(locate, tracerId, traceId, parentSpanId, spanId);
        }

        public static T WithTracerId<T>(this T locate, string tracerId) where T : IClientSpanLocate
        {
            if (locate == null)
            {
                return default(T);
            }

            locate.TracerId = tracerId;
            return locate;
        }

        public static T WithTraceId<T>(this T locate, string traceId) where T : IClientSpanLocate
        {
            if (locate == null)
            {
                return default(T);
            }

            locate.TraceId = traceId;
            return locate;
        }

        public static T WithSpanId<T>(this T locate, string spanId) where T : IClientSpanLocate
        {
            if (locate == null)
            {
                return default(T);
            }

            locate.SpanId = spanId;
            return locate;
        }

        public static T WithParentSpanId<T>(this T locate, string parentSpanId) where T : IClientSpanLocate
        {
            if (locate == null)
            {
                return default(T);
            }

            locate.ParentSpanId = parentSpanId;
            return locate;
        }

        public static bool IsBadLocateArgs(this IClientSpanLocate locate, ClientSpanLocateMode mode)
        {
            return ClientSpanLocateKeyHelper.IsBadLocateArgs(locate, mode);
        }

        public static bool ValidateNewClientSpan(this IClientSpanLocate locate)
        {
            return ClientSpanLocateKeyHelper.ValidateNewClientSpan(locate);
        }
    }

    internal class ClientSpanLocateKeyHelper
    {
        public static string ToLocateCurrentKey(IClientSpanLocate locate)
        {
            if (locate == null)
            {
                return null;
            }
            var locateKey = ToLocateKey(locate.TraceId, locate.SpanId);
            return locateKey;
        }

        public static string ToLocateParentKey(IClientSpanLocate locate)
        {
            if (locate == null)
            {
                return null;
            }
            var locateKey = ToLocateKey(locate.TraceId, locate.ParentSpanId);
            return locateKey;
        }

        public static string ToDisplayKey(IClientSpanLocate locate)
        {
            if (locate == null)
            {
                return null;
            }
            return string.IsNullOrWhiteSpace(locate.ParentSpanId) ?
                string.Format("{0}_{1}_{2}", locate.TracerId, locate.TraceId, locate.SpanId) :
                string.Format("{0}_{1}_{2}_{3}", locate.TracerId, locate.TraceId, locate.ParentSpanId, locate.SpanId);
        }

        public static T With<T>(T locate, string tracerId, string traceId, string parentSpanId, string spanId) where T : IClientSpanLocate
        {
            if (locate == null)
            {
                return default(T);
            }
            locate.TracerId = tracerId;
            locate.TraceId = traceId;
            locate.ParentSpanId = parentSpanId;
            locate.SpanId = spanId;
            return locate;
        }

        public static T With<T>(T locate, IClientSpanLocate setter) where T : IClientSpanLocate
        {
            if (locate == null || setter == null)
            {
                return locate;
            }
            return With(locate, setter.TracerId, setter.TraceId, setter.ParentSpanId, setter.SpanId);
        }

        public static T Create<T>(string tracerId, string traceId, string parentSpanId, string spanId) where T : IClientSpanLocate, new()
        {
            var locate = new T();
            return With(locate, tracerId, traceId, parentSpanId, spanId);
        }

        public static bool IsBadLocateArgs(IClientSpanLocate locate, ClientSpanLocateMode mode)
        {
            return IsBadClientSpan(locate, mode);
        }

        public static bool ValidateNewClientSpan(IClientSpanLocate locate)
        {
            if (locate == null)
            {
                return false;
            }
            return !IsBadClientSpan(locate, ClientSpanLocateMode.ForCurrent);
        }

        private static bool IsBadClientSpan(IClientSpanLocate locate, ClientSpanLocateMode mode)
        {
            if (locate == null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(locate.TracerId))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(locate.TraceId))
            {
                return true;
            }

            if (mode == ClientSpanLocateMode.ForParent)
            {
                if (string.IsNullOrWhiteSpace(locate.ParentSpanId))
                {
                    return true;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(locate.SpanId))
                {
                    return true;
                }
            }

            return false;
        }

        private static string ToLocateKey(string traceId, string spanId)
        {
            if (string.IsNullOrWhiteSpace(traceId) || string.IsNullOrWhiteSpace(spanId))
            {
                return null;
            }
            //for cross services(tracer) use support
            //var locateKey = string.Format("{0}_{1}_{2}", tracerId, traceId, spanId);
            var locateKey = string.Format("{0}_{1}", traceId, spanId);
            return locateKey;
        }
    }
}
