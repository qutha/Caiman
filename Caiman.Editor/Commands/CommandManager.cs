using System.Diagnostics;
using Caiman.Editor.Interfaces;

namespace Caiman.Editor.Commands;

public class CommandManager : ICommandManager, IGuiRender
{
    private readonly Stack<ICommand> _redoStack = new();
    private readonly Stack<ICommand> _undoStack = new();
    private readonly CommandView _view;

    public CommandManager()
    {
        _view = new CommandView(_undoStack, _redoStack);
        _view.Undo += Undo;
        _view.Redo += Redo;
    }


    public void Push(ICommand command)
    {
        try
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
            Debug.WriteLine("Command: " + command.Name);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Command Exception: {ex.Message}");
        }
    }

    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            try
            {
                ICommand redoneCommand = _redoStack.Pop();
                redoneCommand.Execute();
                _undoStack.Push(redoneCommand);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Command Exception: {ex.Message}");
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
                ICommand undoneCommand = _undoStack.Pop();
                undoneCommand.Undo();
                _redoStack.Push(undoneCommand);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Command Exception: {ex.Message}");
            }
        }
        else
        {
            Debug.WriteLine("Undo is empty");
        }
    }

    public void RenderGui()
    {
        _view.RenderGui();
    }
}
