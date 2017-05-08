using System;
using Microsoft.Extensions.Logging;
using RecurrentTasks;

namespace NetTelegramBot.Sample
{
    public class SampleTask : IRunnable
    {
        private readonly ILogger _logger;

        public SampleTask(ILogger<SampleTask> logger)
        {
            _logger = logger;
        }

        public void Run(ITask currentTask)
        {
            _logger.LogInformation($"Time => {DateTime.Now}");
        }
    }
}
