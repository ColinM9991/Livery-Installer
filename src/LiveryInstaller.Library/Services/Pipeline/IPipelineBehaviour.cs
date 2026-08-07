namespace LiveryInstaller.Library.Services.Pipeline;

public interface IPipelineBehaviour<in TContext>
{
    Task ExecuteAsync(TContext context, Func<Task> next, CancellationToken cancellationToken);
}