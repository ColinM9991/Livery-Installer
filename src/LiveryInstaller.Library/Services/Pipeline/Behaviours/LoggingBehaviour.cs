using Microsoft.Extensions.Logging;

namespace LiveryInstaller.Library.Services.Pipeline.Behaviours;

internal sealed class LoggingBehaviour<TContext> : IPipelineBehaviour<TContext>
{
    private readonly ILogger<LoggingBehaviour<TContext>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TContext>> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task ExecuteAsync(TContext context, Func<Task> next, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Invoking pipeline for {Context}", typeof(TContext));

        await next();
        
        _logger.LogDebug("Pipeline completed for {Context}", typeof(TContext));
    }
}