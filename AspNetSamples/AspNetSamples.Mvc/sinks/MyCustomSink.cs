using Serilog.Core;
using Serilog.Events;

namespace AspNetSamples.Mvc.sinks;

public class MyCustomSink : ILogEventSink
{
    private readonly IFormatProvider _formatProvider;

    public MyCustomSink(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(_formatProvider);
        File.AppendAllText("C:\\Users\\AlexiMinor\\Desktop\\434\\full-log.txt", 
            $"{DateTimeOffset.Now.ToString("R")} {message} \n");
    }
}