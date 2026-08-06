namespace LiveryInstaller.Library.Services.Pipeline;

/// <inheritdoc />
public sealed class Pipeline<TContext> : IPipeline<TContext>
{
    private readonly IReadOnlyList<IPipelineStep<TContext>> _steps;

    public Pipeline(IEnumerable<IPipelineStep<TContext>> steps)
    {
        _steps = [.. steps];
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(TContext context, CancellationToken cancellationToken)
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