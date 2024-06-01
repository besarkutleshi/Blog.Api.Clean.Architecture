using Blog.SharedResources;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Blog.Application.Behaviours;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Processing {@RequestName} with Request: {@Request}", 
            requestName,  
            request);

        var response = await next();

        if (response.IsFailure)
        {
            using (LogContext.PushProperty("Error", response.Error, true))
            {
                _logger.LogError("Completed {@RequestName} with {@Error}",
                    requestName,
                    response.Error);
            }
        }

        _logger.LogInformation("Completed {@RequestName} with Response: {@Response}", 
            requestName, 
            response);

        return response;
    }
}
