namespace LiveryInstaller.Library.Services.Pipeline;

/// <summary>
/// Represents a pipeline used for executing operations.
/// </summary>
/// <typeparam name="TContext">The type of the context used by the pipeline.</typeparam>
public interface IPipeline<in TContext>
{
    /// <summary>
    /// Executes the pipeline.
    /// </summary>
    /// <param name="context">The context used by the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the execution.</param>
    /// <returns>A task representing the asynchronous execution of the pipeline.</returns>
    Task ExecuteAsync(TContext context, CancellationToken cancellationToken);
}