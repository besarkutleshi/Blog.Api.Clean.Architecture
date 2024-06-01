using Hangfire;
using System.Linq.Expressions;

namespace Blog.Infrastructure.BackgroundJobs;

public class EnqueueJob : IEnqueueJob
{
    public string Enqueue(Expression<Func<Task>> methodCall)
    {
        var jobId = BackgroundJob.Enqueue(methodCall);

        return jobId;
    }
}