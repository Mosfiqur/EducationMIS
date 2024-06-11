using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using UnicefEducationMIS.Core.Models;

namespace UnicefEducationMIS.Data.Logging
{
    public class CustomLogger : ILogger
    {
        public CustomLogger(LoggerProvider provider, string category)
        {
            this.Provider = provider;
            this.Category = category;
        }

        public LoggerProvider Provider { get; private set; }
        public string Category { get; private set; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if ((this as ILogger).IsEnabled(logLevel))
            {
                LogEntry log = new LogEntry();
                log.TimeStampUtc = DateTime.Now;
                log.Category = this.Category;
                log.Level = logLevel;
                // well, the passed default formatter function 
                // does not take the exception into account
                // SEE: https://github.com/aspnet/Extensions/blob/master/src/Logging/Logging.Abstractions/src/LoggerExtensions.cs
                log.Text = exception?.Message ?? state.ToString(); // formatter(state, exception)
                log.Exception = exception;
                log.EventId = eventId;
                log.State = state;

                // well, you never know what it really is
                if (state is string)
                {
                    log.StateText = state.ToString();
                }
                // in case we have to do with a message template, 
                // let's get the keys and values (for Structured Logging providers)
                // SEE: https://docs.microsoft.com/en-us/aspnet/core/
                // fundamentals/logging#log-message-template
                // SEE: https://softwareengineering.stackexchange.com/
                // questions/312197/benefits-of-structured-logging-vs-basic-logging
                else if (state is IEnumerable<KeyValuePair<string, object>> Properties)
                {
                    log.StateProperties = new Dictionary<string, object>();

                    foreach (KeyValuePair<string, object> item in Properties)
                    {
                        log.StateProperties[item.Key] = item.Value;
                    }
                }

                // gather info about scope(s), if any
                if (Provider.ScopeProvider != null)
                {
                    Provider.ScopeProvider.ForEachScope((value, loggingProps) =>
                    {
                        if (log.Scopes == null)
                            log.Scopes = new List<LogScopeInfo>();

                        LogScopeInfo Scope = new LogScopeInfo();
                        log.Scopes.Add(Scope);

                        if (value is string)
                        {
                            Scope.Text = value.ToString();
                        }
                        else if (value is IEnumerable<KeyValuePair<string, object>> props)
                        {
                            if (Scope.Properties == null)
                                Scope.Properties = new Dictionary<string, object>();

                            foreach (var pair in props)
                            {
                                Scope.Properties[pair.Key] = pair.Value;
                            }
                        }
                    },
                    state);
                }

                Provider.WriteLog(log);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return Provider.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return Provider.ScopeProvider.Push(state);
        }
    }
}
