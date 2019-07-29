namespace CommonTrace.TraceClients
{
    public class TraceResult : IClientSpanLocate
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public string TracerId { get; set; }
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }

        public static TraceResult MethodResult(string method, bool success, object data = null)
        {
            var result = new TraceResult();
            result.Success = success;
            result.Data = data;
            result.With(data as IClientSpanLocate);
            result.Message = string.Format("{0}: {1} => {2}", method, result.ToDisplayKey(), success ? " Success" : " Fail");
            return result;
        }

        public static TraceResult SuccessResult(string message, object data = null)
        {
            var result = new TraceResult() { Message = message, Success = true, Data = data };
            result.With(data as IClientSpanLocate);
            return result;
        }

        public static TraceResult FailResult(string message, object data = null)
        {
            var result = new TraceResult() { Message = message, Success = false, Data = data };
            result.With(data as IClientSpanLocate);
            return result;
        }

    }
}
