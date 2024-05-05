using System.Numerics;
using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor.Commands;

public class CommandView : IGuiRender
{
    private readonly IEnumerable<ICommand> _redoStack;
    private readonly IEnumerable<ICommand> _undoStack;

    public CommandView(IEnumerable<ICommand> undoStack, IEnumerable<ICommand> redoStack)
    {
        _undoStack = undoStack;
        _redoStack = redoStack;
    }

    #region IGuiRender Members

    public void RenderGui()
    {
        ImGui.Begin("Commands");
        if (ImGui.Button("Undo"))
        {
            Undo?.Invoke();
        }

        ImGui.SameLine();
        if (ImGui.Button("Redo"))
        {
            Redo?.Invoke();
        }

        ImGui.SameLine();
        if (ImGui.Button("Clear"))
        {
            Clear?.Invoke();
        }

        foreach (var command in _redoStack.Reverse())
        {
            ImGui.TextColored(new Vector4(1, 0, 0, 1), command.Name);
        }

        foreach (var command in _undoStack)
        {
            ImGui.TextColored(new Vector4(0, 0.5f, 0, 1), command.Name);
        }


        ImGui.End();
    }

    #endregion

    public event Action? Undo;
    public event Action? Redo;
    public event Action? Clear;
}
