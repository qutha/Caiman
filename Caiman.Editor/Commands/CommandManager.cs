using System.Diagnostics;
using Caiman.Editor.Interfaces;

namespace Caiman.Editor.Commands;

public class CommandManager : ICommandManager, IGuiRender
{
    private readonly EditorConsole _console;
    private readonly Stack<ICommand> _redoStack = new();
    private readonly Stack<ICommand> _undoStack = new();
    private readonly CommandView _view;

    public CommandManager(EditorConsole console)
    {
        _console = console;
        _view = new CommandView(_undoStack, _redoStack);
        _view.Undo += Undo;
        _view.Redo += Redo;
        _view.Clear += Clear;
    }

    #region ICommandManager Members

    public void Push(ICommand command)
    {
        try
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
            var message = $"Command: {command.Name}";
            _console.WriteLine(message);
            Debug.WriteLine(message);
        }
        catch (Exception ex)
        {
            var message = $"Command: {ex.Message}";
            _console.WriteLine(message);
            Debug.WriteLine(message);
        }
    }

    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            try
            {
                var redoCommand = _redoStack.Pop();
                redoCommand.Execute();
                _undoStack.Push(redoCommand);
                var message = $"Redo: {redoCommand.Name}";
                _console.WriteLine(message);
                Debug.WriteLine(message);
            }
            catch (Exception ex)
            {
                var message = $"Command: {ex.Message}";
                _console.WriteLine(message);
                Debug.WriteLine(message);
            }
        }
        else
        {
            Debug.WriteLine("Redo is empty");
        }
    }

    public void Undo()
    {
        if (_undoStack.Count > 0)
        {
            try
            {
                var undoneCommand = _undoStack.Pop();
                undoneCommand.Undo();
                _redoStack.Push(undoneCommand);
                var message = $"Undo: {undoneCommand.Name}";
                _console.WriteLine(message);
                Debug.WriteLine(message);
            }
            catch (Exception ex)
            {
                var message = $"Command: {ex.Message}";
                _console.WriteLine(message);
                Debug.WriteLine(message);
            }
        }
        else
        {
            Debug.WriteLine("Undo is empty");
        }
    }

    #endregion

    #region IGuiRender Members

    public void RenderGui() => _view.RenderGui();

    #endregion

    private void Clear()
    {
        _undoStack.Clear();
        _redoStack.Clear();
    }
}
