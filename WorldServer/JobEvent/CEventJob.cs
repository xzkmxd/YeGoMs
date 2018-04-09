﻿using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorldServer.JobEvent
{
    public class CEventJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            System.Console.WriteLine($"Hello World! - {DateTime.Now:r}");
            return Task.FromResult(true);
        }
    }
}
