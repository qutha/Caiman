using Caiman.Core.Analysis;
using Caiman.Core.Construction;
using Caiman.Core.Optimization.Restrictions;
using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor.Construction.Restrictions;

public class ConstructionRestrictionsWindow(
    RestrictionsState restrictionsState,
    EditorConstruction editorConstruction,
    ConstructionAnalyzer analyzer,
    ConstructionManager constructionManager)
    : IGuiRender
{
    private readonly AreaRestrictionState _areaRestrictionState = new();
    private Axis _axis = Axis.X;
    private Core.Construction.Construction _construction;
    private double _maxDisplacement;
    private double _minAreaForAll;
    private int _nodeId;

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
            _construction = constructionManager.ToConstruction(editorConstruction);
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
                    restrictionsState.Restrictions.Add(new AreaRestriction(_areaRestrictionState.ElementId,
                        _areaRestrictionState.MinArea));
                }

                ImGui.SeparatorText("Areas Restriction");
                ImGui.InputDouble("Min Area For All", ref _minAreaForAll);
                ImGui.PushID("Areas Restriction");
                if (ImGui.Button("Add"))
                {
                    restrictionsState.Restrictions.RemoveAll(r => r is AreaRestriction);
                    restrictionsState.Restrictions.AddRange(
                        AreaRestriction.CreateRestrictionForAll(_construction, _minAreaForAll));
                }

                ImGui.SeparatorText("Node Displacement");
                ImGui.InputInt("Node Id", ref _nodeId);
                ImGui.InputDouble("Max Displacement", ref _maxDisplacement);
                // ImGui.RadioButton("Max Displacement", ref _maxDisplacement);
                ImGui.PushID("Node Displacement Restriction");
                if (ImGui.Button("Add"))
                {
                    Func<IList<double>, double> func = analyzer.GenerateNodeDisplacementOnAreasFunction(_construction,
                        _construction.Nodes[_nodeId], Axis.Y);
                    restrictionsState.Restrictions.Add(new NodeDisplacementRestriction(null, _maxDisplacement));
                }

                ImGui.EndPopup();
            }

            ImGui.SeparatorText("Restrictions");
            foreach (var restriction in restrictionsState.Restrictions)
            {
                ImGui.Text(restriction.ToString());
                ImGui.PushID(restriction.ToString());
                if (ImGui.Button("Remove")) { }
            }

            ImGui.End();
        }
    }

    #endregion

    #region Nested type: AreaRestrictionState

    private class AreaRestrictionState
    {
        public int ElementId;
        public double MinArea;
    }

    #endregion
}
