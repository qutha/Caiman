namespace Caiman.Editor.Commands;

public interface ICommand
{
    public string Name { get; }
    void Execute();
    void Undo();
}
