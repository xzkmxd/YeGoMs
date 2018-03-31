using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.App
{
    public class HelloJob : IJob
    {
        Task IJob.Execute(IJobExecutionContext context)
        {
            System.Console.WriteLine($"Hello World! - {DateTime.Now:r}");
            return Task.FromResult(true);
        }
    }
}
