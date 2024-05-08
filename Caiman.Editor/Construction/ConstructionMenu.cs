using System.Diagnostics;
using Caiman.Editor.Construction.Restrictions;
using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor.Construction;

public class ConstructionMenu(
    EditorConsole console,
    ConstructionManager constructionManager,
    CalculationResults calculationResults,
    ConstructionRestrictionsWindow restrictionsWindow) : IMenu
{
    #region IMenu Members

    public void RenderMenu()
    {
        if (ImGui.BeginMenu("Construction"))
        {
            if (ImGui.MenuItem("Set Restrictions"))
            {
                restrictionsWindow.IsOpen = !restrictionsWindow.IsOpen;
            }

            if (ImGui.MenuItem("Internal Forces"))
            {
                _ = Task.Run(() =>
                {
                    console.WriteLine("Internal forces...");
                    var clock = new Stopwatch();
                    clock.Start();
                    var result = constructionManager.FindInternalForces();
                    clock.Stop();
                    calculationResults.Write(result);
                    console.WriteLine($"Internal forces task done in {clock.ElapsedMilliseconds}ms");
                });
            }

            if (ImGui.MenuItem("Stresses"))
            {
                _ = Task.Run(() =>
                {
                    console.WriteLine("Stresses...");
                    var clock = new Stopwatch();
                    clock.Start();
                    var result = constructionManager.FindStresses();
                    clock.Stop();
                    calculationResults.Write(result);
                    console.WriteLine($"Stresses task done in {clock.ElapsedMilliseconds}ms");
                });
            }


            if (ImGui.MenuItem("Nodal displacement"))
            {
                _ = Task.Run(() =>
                {
                    console.WriteLine("Nodal displacement...");
                    var clock = new Stopwatch();
                    clock.Start();
                    var result = constructionManager.FindNodalDisplacement();
                    clock.Stop();
                    calculationResults.Write(result);
                    console.WriteLine($"Nodal displacement task done in {clock.ElapsedMilliseconds}ms");
                });
            }

            if (ImGui.MenuItem("Material consumption optimization"))
            {
                _ = Task.Run(() =>
                {
                    console.WriteLine("Material consumption optimization...");
                    var clock = new Stopwatch();
                    clock.Start();
                    var result = constructionManager.Optimize();
                    clock.Stop();
                    calculationResults.Write(result);
                    console.WriteLine(
                        $"Material consumption optimization task done in {clock.ElapsedMilliseconds}ms");
                });
            }

            if (ImGui.MenuItem("Areas selection"))
            {
                _ = Task.Run(() =>
                {
                    console.WriteLine("Areas selection...");
                    var clock = new Stopwatch();
                    clock.Start();
                    var result = constructionManager.SelectDiscreteAreas();
                    clock.Stop();
                    calculationResults.Write(result);
                    console.WriteLine($"Areas selection task done in {clock.ElapsedMilliseconds}ms");
                });
            }

            ImGui.EndMenu();
        }
    }

    #endregion
}
