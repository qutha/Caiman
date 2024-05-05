using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor.ConstructionOptions;

public class ConstructionOptionsMenu : IMenu
{
    #region IMenu Members

    public void RenderMenu()
    {
        if (ImGui.Button("Construction Options")) { }
    }

    #endregion
}
