using System.Linq.Expressions;

namespace Blog.Infrastructure.BackgroundJobs;

public interface IEnqueueJob
{
    string Enqueue(Expression<Func<Task>> methodCall);
}
