using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UnicefEducationMIS.Core.ApplicationServices;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Repositories;
using UnicefEducationMIS.Data.Repositories;

namespace UnicefEducationMIS.Data.Logging
{
    [Microsoft.Extensions.Logging.ProviderAlias("Database")]
    public class DbLoggerProvider : LoggerProvider
    {
        private readonly IDbContextService _contextService;
        private bool terminated;
        ConcurrentQueue<LogEntry> InfoQueue = new ConcurrentQueue<LogEntry>();

        public DbLoggerProvider(IDbContextService contextService)
        {
            _contextService = contextService;
            ThreadProc();
        }

        private void ThreadProc()
        {
            Task.Run(() =>
            {
                while (!terminated)
                {
                    try
                    {
                        WriteLogLine();
                        Thread.Sleep(100);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
        }

        private void WriteLogLine()
        {
            if (InfoQueue.Count == 0) return;
            var dbContext = _contextService.GetContext();
            if (dbContext == null) return;
            if (InfoQueue.TryDequeue(out var log))
            {
                var repo = new DbLoggingRepository(dbContext);
                repo.Insert(log);
            }
        }

        public override bool IsEnabled(LogLevel logLevel)
        {
            //TODO: implement later 
            return true;
        }

        public override void WriteLog(LogEntry log)
        {
            InfoQueue.Enqueue(log);
        }

        protected override void Dispose(bool disposing)
        {
            terminated = true;
            base.Dispose(disposing);
        }
    }
}
