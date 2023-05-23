namespace Inflow.Shared.Abstractions.Commands;

public interface ICommand
{
}

public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}