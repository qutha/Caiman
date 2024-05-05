using Caiman.Editor.Interfaces;

namespace Caiman.Editor;

public class QueueManager : IRender, IWorldRender, IGuiRender, IUpdate
{
    public QueueManager(IEnumerable<IQueueManaged> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    private List<IRender> RenderQueue { get; } = [];
    private List<IWorldRender> WorldRenderQueue { get; } = [];
    private List<IGuiRender> GuiRenderQueue { get; } = [];
    private List<IUpdate> UpdateQueue { get; } = [];

    #region IGuiRender Members

    public void RenderGui()
    {
        foreach (var item in GuiRenderQueue)
        {
            item.RenderGui();
        }
    }

    #endregion

    #region IRender Members

    public void Render()
    {
        foreach (var item in RenderQueue)
        {
            item.Render();
        }
    }

    #endregion

    #region IUpdate Members

    public void Update()
    {
        foreach (var item in UpdateQueue)
        {
            item.Update();
        }
    }

    #endregion

    #region IWorldRender Members

    public void RenderWorld()
    {
        foreach (var item in WorldRenderQueue)
        {
            item.RenderWorld();
        }
    }

    #endregion

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
