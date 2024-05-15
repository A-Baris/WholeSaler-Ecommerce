﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Business.Logger
{
    public interface ILogger
    {
        void LogInformation(string message);
        void LogError(string message, Exception ex);
    }
}
