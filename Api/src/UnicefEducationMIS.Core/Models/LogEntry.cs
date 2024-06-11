using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace UnicefEducationMIS.Core.Models
{
    public class LogEntry:BaseModel<int> 
    {
        public LogEntry()
        {
            TimeStampUtc = DateTime.UtcNow;
            UserName = Environment.UserName;
        }

        public string UserName { get; set; }
        public DateTime TimeStampUtc { get; set; }
        public string Category { get; set; }
        public LogLevel Level { get; set; }
        public string Text { get; set; }
        public Exception Exception { get; set; }
        public EventId EventId { get; set; }
        public object State { get; set; }
        public string StateText { get; set; }
        public Dictionary<string, object> StateProperties { get; set; }
        public List<LogScopeInfo> Scopes { get; set; }
    }
}
