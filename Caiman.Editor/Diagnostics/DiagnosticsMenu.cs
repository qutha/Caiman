using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor.Diagnostics;

public class DiagnosticsMenu(DiagnosticsWindow window) : IMenu
{
    #region IMenu Members

    public void RenderMenu()
    {
        if (ImGui.Button("Diagnostics"))
        {
            window.IsOpen = !window.IsOpen;
        }

        // ImGui.EndMenu();
    }

    #endregion
}
