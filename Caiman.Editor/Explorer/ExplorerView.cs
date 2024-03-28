using Caiman.Core.Depr.Construction;
using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor.Explorer;

public class ExplorerView(ConstructionModel constructionModel) : IGuiRender
{
    public void RenderGui()
    {
        ImGui.Begin("Explorer");
        foreach (Node node in constructionModel.Nodes.ToList())
        {
            ImGui.Text(node.ToString());
            ImGui.SameLine();
            foreach (ConcentratedLoad load in node.Loads)
            {
                ImGui.Text($"Load: {load.Value}");
            }

            ImGui.SameLine();

            ImGui.PushID(node.ToString());
            if (ImGui.Button("Remove"))
            {
                RemoveNode?.Invoke(node.Id);
            }

            ImGui.PopID();
        }

        foreach (Element element in constructionModel.Elements.ToList())
        {
            ImGui.Text(element.ToString());
            ImGui.SameLine();
            ImGui.PushID(element.ToString());
            if (ImGui.Button("Remove"))
            {
                RemoveElement?.Invoke(element.Id);
            }

            ImGui.PopID();
        }

        ImGui.End();
    }


    public event Action<int>? RemoveNode;
    public event Action<int>? RemoveElement;
}
