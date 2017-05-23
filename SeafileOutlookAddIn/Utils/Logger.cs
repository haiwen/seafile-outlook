using System;
using System.Collections.Generic;
using System.Text;
using log4net.Config;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Layout;
using log4net.Core;

namespace SeafileOutlookAddIn.Utils
{
    class Logger
    {
        public static void ConfigureFileAppender(string logFile)
        {
            var fileAppender = GetFileAppender(logFile);
            BasicConfigurator.Configure(fileAppender);
            ((Hierarchy)LogManager.GetRepository()).Root.Level = Level.Debug;
        }

        private static IAppender GetFileAppender(string logFile)
        {
            var layout = new PatternLayout("%date %-5level %logger - %message%newline");
            layout.ActivateOptions(); // According to the docs this must be called as soon as any properties have been changed.

            var appender = new FileAppender
            {
                File = logFile,
                Encoding = Encoding.UTF8,
                Threshold = Level.Debug,
                Layout = layout
            };

            appender.ActivateOptions();

            return appender;
        }
    }
}
