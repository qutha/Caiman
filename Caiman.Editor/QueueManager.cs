using Caiman.Editor.Interfaces;

namespace Caiman.Editor;

public class QueueManager : IRender, IWorldRender, IGuiRender, IUpdate
{
    public List<IRender> RenderQueue { get; } = [];
    public List<IWorldRender> WorldRenderQueue { get; } = [];
    public List<IGuiRender> GuiRenderQueue { get; } = [];
    public List<IUpdate> UpdateQueue { get; } = [];

    public void RenderGui()
    {
        foreach (IGuiRender item in GuiRenderQueue)
        {
            item.RenderGui();
        }
    }

    public void Render()
    {
        foreach (IRender item in RenderQueue)
        {
            item.Render();
        }
    }

    public void Update()
    {
        foreach (IUpdate item in UpdateQueue)
        {
            item.Update();
        }
    }

    public void RenderWorld()
    {
        foreach (IWorldRender item in WorldRenderQueue)
        {
            item.RenderWorld();
        }
    }

    public void Add<T>(T item) where T : IQueueManaged
    {
        if (item is IGuiRender guiRender)
        {
            GuiRenderQueue.Add(guiRender);
        }

        if (item is IRender render)
        {
            RenderQueue.Add(render);
        }

        if (item is IUpdate update)
        {
            UpdateQueue.Add(update);
        }

        if (item is IWorldRender worldRender)
        {
            WorldRenderQueue.Add(worldRender);
        }
    }
}
