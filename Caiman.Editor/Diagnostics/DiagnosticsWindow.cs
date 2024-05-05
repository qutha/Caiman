using Caiman.Editor.Interfaces;
using ImGuiNET;
using Raylib_cs;

namespace Caiman.Editor.Diagnostics;

public class DiagnosticsWindow : IGuiRender
{
    private readonly Timer _timer;

    private long _allocatedBytes;

    public bool IsOpen;

    public DiagnosticsWindow() => _timer = new Timer(OnTimerTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

    #region IGuiRender Members

    public void RenderGui()
    {
        if (!IsOpen)
        {
            return;
        }

        if (ImGui.Begin("Diagnostics", ref IsOpen))
        {
            ImGui.Text($"Frame time: {GetFrameTime()} ms");
            ImGui.Text($"Allocated: {_allocatedBytes} kb");
        }

        ImGui.End();
    }

    #endregion


    private void OnTimerTick(object? _) => _allocatedBytes = GetAllocatedKilobytes();

    private long GetAllocatedKilobytes() => GC.GetTotalAllocatedBytes() / 1000;

    private float GetFrameTime() => Raylib.GetFrameTime() * 1000;
}
