using System.Text;
using Caiman.Editor.Interfaces;
using ImGuiNET;
using Raylib_cs;

namespace Caiman.Editor;

public class CalculationResults : IGuiRender
{
    private readonly StringBuilder _message = new();

    #region IGuiRender Members

    public void RenderGui()
    {
        if (ImGui.Begin("Calculation Results"))
        {
            if (ImGui.Button("Copy"))
            {
                Raylib.SetClipboardText(_message.ToString());
            }

            ImGui.SameLine();
            if (ImGui.Button("Clear"))
            {
                Clear();
            }

            ImGui.Separator();
            if (ImGui.BeginChild("Output"))
            {
                ImGui.Text(_message.ToString());
            }
        }

        ImGui.End();
    }

    #endregion

    public void Clear() => _message.Clear();

    public void Write(string text) => _message.Append(text);

    public void WriteLine(string line) => _message.AppendLine(line);
}
