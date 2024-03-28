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

    public Action? Undo { get; set; }
    public Action? Redo { get; set; }

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

        foreach (ICommand command in _redoStack)
        {
            ImGui.TextColored(new Vector4(1, 0, 0, 1), command.Name);
        }

        foreach (ICommand command in _undoStack)
        {
            ImGui.TextColored(new Vector4(0, 0.5f, 0, 1), command.Name);
        }


        ImGui.End();
    }
}
