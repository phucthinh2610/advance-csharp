using advance_csharp.service.Interface;

using log4net;

namespace advance_csharp.service.Service
{
    public class LoggingService : ILoggingService
    {
        private readonly ILog _logger;
        public LoggingService()
        {
            _logger = LogManager.GetLogger(typeof(LoggingService));
        }

        /// <summary>
        /// LogError
        /// </summary>
        /// <param name="exception"></param>
        public void LogError(Exception exception)
        {
            _logger.Error(exception);
        }

        /// <summary>
        /// LogInfo
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(string message)
        {
            _logger.Info(message);
        }


    }
}