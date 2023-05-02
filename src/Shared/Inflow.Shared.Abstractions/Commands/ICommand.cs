namespace Inflow.Shared.Abstractions.Commands;

public interface ICommand
{
}

public interface ICommandHandlers<in TCommand> where TCommand : class, ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}