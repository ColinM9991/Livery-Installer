namespace LiveryInstaller.Library.Services.Pipeline;
 
/// <summary>
/// Represents an undoable step in the pipeline.
/// </summary>
/// <typeparam name="TContext">The type of the context.</typeparam>
internal interface ICompensatableStep<in TContext>
{
    /// <summary>
    /// Undoes a previously executed step.
    /// </summary>
    /// <param name="context">The <typeparam name="TContext"/>.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UndoAsync(TContext context, CancellationToken cancellationToken);
}