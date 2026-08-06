namespace LiveryInstaller.Library.Services.Pipeline;

/// <summary>
/// Represents a step in a pipeline.
/// </summary>
/// <typeparam name="TContext">The type of the context used by the pipeline step.</typeparam>
public interface IPipelineStep<in TContext>
{
    /// <summary>
    /// Executes the pipeline step.
    /// </summary>
    /// <param name="context">The context used by the pipeline step.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns>A task representing the asynchronous execution of the pipeline step.</returns>
    Task ExecuteAsync(TContext context, CancellationToken cancellationToken);
}