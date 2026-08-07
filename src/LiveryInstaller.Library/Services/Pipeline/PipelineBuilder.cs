using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LiveryInstaller.Library.Services.Pipeline;

/// <inheritdoc />
internal class PipelineBuilder<TContext> : IPipelineBuilder<TContext>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PipelineBuilder<TContext>> _logger;

    private readonly ICollection<IPipelineStep<TContext>> _steps;
    private readonly Stack<IPipelineBehaviour<TContext>> _behaviours;

    public PipelineBuilder(
        IServiceProvider serviceProvider,
        ILogger<PipelineBuilder<TContext>> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;

        _steps = [];
        _behaviours = [];
    }

    /// <inheritdoc />
    public IPipelineBuilder<TContext> AddStep<TStep>() where TStep : class, IPipelineStep<TContext>
    {
        _logger.LogDebug("Adding step {StepType}", typeof(TStep));
        _steps.Add(_serviceProvider.GetRequiredService<TStep>());

        return this;
    }
    
    public IPipelineBuilder<TContext> AddBehaviour<TBehaviour>() where TBehaviour : class, IPipelineBehaviour<TContext>
    {
        _logger.LogDebug("Adding behaviour {BehaviourType}", typeof(TBehaviour));
        _behaviours.Push(_serviceProvider.GetRequiredService<TBehaviour>());

        return this;
    }

    /// <inheritdoc />
    public IPipeline<TContext> Build()
    {
        return _steps.Count == 0
            ? throw new InvalidOperationException("No steps added to the pipeline")
            : new Pipeline<TContext>(_steps, _behaviours);
    }
}