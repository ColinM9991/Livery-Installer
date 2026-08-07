using System.Collections.Immutable;

namespace LiveryInstaller.Library.Services.Pipeline;

/// <inheritdoc />
public sealed class Pipeline<TContext> : IPipeline<TContext>
{
    private readonly IReadOnlyList<IPipelineStep<TContext>> _steps;
    private readonly Stack<IPipelineBehaviour<TContext>> _behaviours;

    internal Pipeline(
        IEnumerable<IPipelineStep<TContext>> steps,
        Stack<IPipelineBehaviour<TContext>> behaviours)
    {
        _steps = [.. steps];
        _behaviours = behaviours;
    }

    /// <inheritdoc />
    public Task ExecuteAsync(TContext context, CancellationToken cancellationToken)
    {
        var pipeline = () => ExecuteStepsAsync(context, cancellationToken);

        while (_behaviours.TryPop(out var behaviour))
        {
            var next = pipeline;
            
            pipeline = () => behaviour.ExecuteAsync(context, next, cancellationToken);
        }

        return pipeline();
    }

    private async Task ExecuteStepsAsync(TContext context, CancellationToken cancellationToken)
    {
        Stack<ICompensatableStep<TContext>> compensatableSteps = [];

        try
        {
            foreach (var step in _steps)
            {
                await step.ExecuteAsync(context, cancellationToken);

                if (step is ICompensatableStep<TContext> compensatableStep)
                {
                    compensatableSteps.Push(compensatableStep);
                }
            }
        }
        catch
        {
            while (compensatableSteps.TryPop(out var compensatableStep))
            {
                try
                {
                    await compensatableStep.UndoAsync(context, CancellationToken.None);
                }
                catch
                {
                }
            }

            throw;
        }
    }
}