using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Tixora.Shared.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> 
    where TRequest: IRequest<TResponse>
    where TResponse: IErrorOr
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string moduleName = GetModuleName(typeof(TRequest));
        string requestName = typeof(TRequest).Name;

        using (LogContext.PushProperty("Module", moduleName))
        {
            _logger.LogInformation("Processing request {RequestName}", requestName);

            TResponse result = await next(cancellationToken);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Completed request {RequestName}", requestName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Errors, true))
                {
                    _logger.LogError("Completed request {RequestName} with error", requestName);
                }
            }

            return result;
        }
    }
    
    private static string GetModuleName(Type type) => type.FullName!.Split('.')[2];
}