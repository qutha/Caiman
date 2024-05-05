using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor.Menu;

public class MenuBar(IEnumerable<IMenu> menuItems) : IGuiRender
{
    #region IGuiRender Members

    public void RenderGui()
    {
        if (ImGui.BeginMainMenuBar())
        {
            foreach (var menuItem in menuItems)
            {
                menuItem.RenderMenu();
            }
        }

        ImGui.EndMainMenuBar();
    }

    #endregion
}
