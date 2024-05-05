using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor;

public class EditorConsole : IGuiRender
{
    private const int MaxSize = 100;
    private readonly List<string> _messages = new(MaxSize);

    #region IGuiRender Members

    public void RenderGui()
    {
        if (ImGui.Begin("Console"))
        {
            if (ImGui.Button("Clear Console"))
            {
                Clear();
            }

            ImGui.Separator();
            if (ImGui.BeginChild("Console Output"))
            {
                foreach (var message in _messages)
                {
                    ImGui.Text(message);
                }
            }
        }

        ImGui.End();
    }

    #endregion

    public void WriteLine(string message)
    {
        if (_messages.Count > MaxSize)
        {
            Clear();
        }

        _messages.Add(message);
    }

    public void Clear() => _messages.Clear();
}
