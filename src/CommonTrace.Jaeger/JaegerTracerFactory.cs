using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CommonTrace.OpenTraces;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
using Microsoft.Extensions.Logging;
using OpenTracing;

namespace CommonTrace.Jaeger
{
    public class JaegerTracerFactory : ITracerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        public JaegerTracerFactory(ILoggerFactory loggerFactory, TraceConfig config)
        {
            _loggerFactory = loggerFactory;
            Config = config;
            CachedTracers = new ConcurrentDictionary<string, ITracer>(StringComparer.OrdinalIgnoreCase);
        }

        public TraceConfig Config { get; set; }

        public ITracer CreateTracer(string tracerId)
        {
            return CreateTracer(Config.GetTraceEndPoint(), tracerId);
        }

        public IDictionary<string, ITracer> CachedTracers { get; }

        private Tracer CreateTracer(string endPoint, string serviceName)
        {
            var traceBuilder = new Tracer.Builder(serviceName)
                .WithSampler(new ConstSampler(true))
                .WithLoggerFactory(_loggerFactory);

            //var loggerFactory = traceBuilder.LoggerFactory;
            var metrics = traceBuilder.Metrics;

            //14268:OK
            var sender = new HttpSender(endPoint);

            var reporter = new RemoteReporter.Builder()
                .WithLoggerFactory(_loggerFactory)
                .WithMetrics(metrics)
                .WithSender(sender)
                .Build();

            var tracer = traceBuilder
                .WithReporter(reporter)
                .Build();

            return tracer;
        }
    }
}
