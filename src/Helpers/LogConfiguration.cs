﻿using System;
using System.Diagnostics;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Helpers {
    public class LogConfiguration : ILogConfiguration {
        public string LogSubFolder => @"AspenlaubLogs\Tash";
        public string LogId => $"{DateTime.Today:yyyy-MM-dd}-{Process.GetCurrentProcess().Id}";
    }
}
