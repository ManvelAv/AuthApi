using Serilog;

namespace MainApi.Logger
{
    public class LoggerConfig
    {
        static LoggerConfig()
        {
            string logFilePath = "C:\\Users\\Mle\\Desktop\\MainApi.txt";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static void LogDebug(string message)
        {
            Log.Debug(message);
        }

        public static void LogInformation(string message)
        {
            Log.Information(message);
        }

        public static void LogWarning(string message)
        {
            Log.Warning(message);
        }

        public static void LogError(string message)
        {
            Log.Error(message);
        }

        public static void LogFatal(string message)
        {
            Log.Fatal(message);
        }

        public static void CloseLogger()
        {
            Log.CloseAndFlush();
        }

    }
}
