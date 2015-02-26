using System;

namespace DLog.NET.Models
{
    public class LogMessage
    {
        private readonly DateTime _timestamp;
        private readonly string _message;

        public DateTime TimeStamp { get { return _timestamp; } }
        public string Message { get { return _message; } }

        public LogMessage(string message)
            : this()
        {
            _message = message;
        }

        private LogMessage()
        {
            _timestamp = DateTime.Now;
            _message = "";
        }

        public string GetFormatted()
        {
            return string.Format("[{0:d/M/yyyy HH:mm:ss}]: {1}", TimeStamp, Message);
        }
    }
}
