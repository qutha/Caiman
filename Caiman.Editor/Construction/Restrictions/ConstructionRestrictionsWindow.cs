using Caiman.Core.Construction;
using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor.Construction.Restrictions;

public class ConstructionRestrictionsWindow(
    EditorConstruction editorConstruction,
    RestrictionsState state)
    : IGuiRender
{
    private readonly AreaRestrictionState _areaRestrictionState = new();
    private readonly NodeDisplacementRestrictionState _nodeDisplacementRestrictionState = new();
    private Axis _axis = Axis.X;
    private double _minForAll;

    public bool IsOpen;

    #region IGuiRender Members

    //TODO проверить везде Begin End
    public void RenderGui()
    {
        if (!IsOpen)
        {
            return;
        }

        if (ImGui.Begin("Set Restrictions", ref IsOpen))
        {
            if (ImGui.Button("Add Restriction"))
            {
                ImGui.OpenPopup("Restriction Type");
            }

            if (ImGui.BeginPopup("Restriction Type"))
            {
                ImGui.SeparatorText("Area Restriction");
                ImGui.InputInt("Element Id", ref _areaRestrictionState.ElementId);
                ImGui.InputDouble("Min Area", ref _areaRestrictionState.MinArea);
                ImGui.PushID("Area Restriction");
                if (ImGui.Button("Add"))
                {
                    state.AreaRestrictionStates.Add(new AreaRestrictionState
                    {
                        ElementId = _areaRestrictionState.ElementId,
                        MinArea = _areaRestrictionState.MinArea,
                    });
                }

                ImGui.SeparatorText("Areas Restriction");
                ImGui.InputDouble("Min Area For All", ref _minForAll);
                ImGui.PushID("Areas Restriction");
                if (ImGui.Button("Add"))
                {
                    state.AreaRestrictionStates.Clear();
                    state.AreaRestrictionStates.AddRange(editorConstruction.Elements.Select(el =>
                        new AreaRestrictionState
                        {
                            ElementId = el.Id,
                            MinArea = _minForAll,
                        }));
                }

                ImGui.SeparatorText("Node Displacement");
                ImGui.InputInt("Node Id", ref _nodeDisplacementRestrictionState.NodeId);
                ImGui.InputDouble("Max Displacement", ref _nodeDisplacementRestrictionState.MaxDisplacement);
                if (ImGui.RadioButton("X Axis", _axis == Axis.X))
                {
                    _axis = Axis.X;
                }

                ImGui.SameLine();
                if (ImGui.RadioButton("Y Axis", _axis == Axis.Y))
                {
                    _axis = Axis.Y;
                }

                ImGui.PushID("Node Displacement Restriction");
                if (ImGui.Button("Add"))
                {
                    state.NodeDisplacementRestrictionStates.Add(new NodeDisplacementRestrictionState
                    {
                        NodeId = _nodeDisplacementRestrictionState.NodeId,
                        MaxDisplacement = _nodeDisplacementRestrictionState.MaxDisplacement,
                        Axis = _axis,
                    });
                }

                ImGui.EndPopup();
            }

            ImGui.SeparatorText("Restrictions");
            foreach (var restriction in state.NodeDisplacementRestrictionStates.ToList())
            {
                ImGui.Text(restriction.ToString());
                ImGui.PushID(restriction.ToString());
                if (ImGui.Button("Remove"))
                {
                    state.NodeDisplacementRestrictionStates.Remove(restriction);
                }
            }

            foreach (var restriction in state.AreaRestrictionStates.ToList())
            {
                ImGui.Text(restriction.ToString());
                ImGui.PushID(restriction.ToString());
                if (ImGui.Button("Remove"))
                {
                    state.AreaRestrictionStates.Remove(restriction);
                }
            }

            ImGui.End();
        }
    }

    #endregion
}
