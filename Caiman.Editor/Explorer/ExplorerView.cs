using Caiman.Editor.Construction;
using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor.Explorer;

public class ExplorerView(EditorConstruction editorConstruction) : IGuiRender
{
    #region IGuiRender Members

    public void RenderGui()
    {
        ImGui.Begin("Explorer");
        foreach (var node in editorConstruction.Nodes.ToList())
        {
            ImGui.Text(node.ToString());

            for (var i = 0; i < node.Loads.Count; i++)
            {
                var load = node.Loads[i];
                ImGui.Text($"\tLoad {i}: {load.X}, {load.Y}");
                ImGui.PushID(load.ToString());
                if (ImGui.Button("Remove Load"))
                {
                    RemoveLoad?.Invoke(node.Id, i);
                }
            }

            ImGui.PushID(node.ToString());
            if (ImGui.Button("Remove Node"))
            {
                RemoveNode?.Invoke(node.Id);
            }

            ImGui.PopID();
        }

        foreach (var element in editorConstruction.Elements.ToList())
        {
            ImGui.Text(element.ToString());
            ImGui.PushID(element.ToString());
            if (ImGui.Button("Remove Element"))
            {
                RemoveElement?.Invoke(element.Id);
            }

            ImGui.PopID();
        }

        ImGui.End();
    }

    #endregion


    public event Action<int>? RemoveNode;
    public event Action<int>? RemoveElement;
    public event Action<int, int>? RemoveLoad;
}
