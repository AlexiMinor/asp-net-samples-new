using Serilog;
using Serilog.Configuration;

namespace AspNetSamples.Mvc.sinks;

    public static class MyCustomSinkExtension
    {
        public static LoggerConfiguration MyCustomSink(
            this LoggerSinkConfiguration loggerConfiguration,
            IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new MyCustomSink(formatProvider));
        }
    }
