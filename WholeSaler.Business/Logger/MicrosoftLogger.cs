
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Business.Logger
{
    public class MicrosoftLogger : ILogger
    {
        private readonly ILogger<MicrosoftLogger> _logger;

    
        public MicrosoftLogger(ILogger<MicrosoftLogger> logger)
        {
           _logger = logger;
        }
        public void LogError(string message, Exception ex)
        {
            _logger.LogError(message, ex);
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
