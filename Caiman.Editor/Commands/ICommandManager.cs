namespace Caiman.Editor.Commands;

public interface ICommandManager
{
    void Push(ICommand command);
    void Redo();
    void Undo();
}
