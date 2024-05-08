using System.Diagnostics;
using System.Numerics;
using Caiman.Core.Construction;
using Caiman.Editor;
using Caiman.Editor.Camera;
using Caiman.Editor.Commands;
using Caiman.Editor.Construction;
using Caiman.Editor.Construction.Commands;
using Caiman.Editor.Construction.Restrictions;
using Caiman.Editor.Diagnostics;
using Caiman.Editor.Explorer;
using Caiman.Editor.Extensions;
using Caiman.Editor.Interfaces;
using Caiman.Editor.Menu;
using Caiman.Editor.Scene;
using Caiman.Editor.Utils;
using Microsoft.Extensions.DependencyInjection;
using ConsoleTraceListener = Caiman.Editor.ConsoleTraceListener;


Trace.Listeners.Add(new ConsoleTraceListener());


var serviceProvider = ConfigureServices();
#if DEBUG

var construction = serviceProvider.GetRequiredService<EditorConstruction>();
var commandManager = serviceProvider.GetRequiredService<CommandManager>();
commandManager.Push(new AddNodeCommand(construction, new Vector2(0, 700)));
commandManager.Push(new AddNodeCommand(construction, new Vector2(1200, 900)));
commandManager.Push(new AddNodeCommand(construction, new Vector2(1400, 0)));
commandManager.Push(new AddNodeCommand(construction, new Vector2(0, 0)));
const double elasticity = 2_000_000;
const int area = 14;
commandManager.Push(new AddElementCommand(construction, 0, 1, elasticity, area));
commandManager.Push(new AddElementCommand(construction, 1, 2, elasticity, area));
commandManager.Push(new AddElementCommand(construction, 2, 3, elasticity, area));
commandManager.Push(new AddElementCommand(construction, 3, 0, elasticity, area));
commandManager.Push(new AddElementCommand(construction, 0, 2, elasticity, area));
commandManager.Push(new AddElementCommand(construction, 3, 1, elasticity, area));
commandManager.Push(new AddLoadCommand(construction, 1, new Vector2(10_000, 0)));
commandManager.Push(new AddConstraintCommand(construction, 2, false, true));
commandManager.Push(new AddConstraintCommand(construction, 3, true, true));

var restrictionsState = serviceProvider.GetRequiredService<RestrictionsState>();
restrictionsState.AreaRestrictionStates.AddRange(construction.Elements.Select(el =>
    new AreaRestrictionState
    {
        ElementId = el.Id,
        MinArea = 10,
    }));
restrictionsState.NodeDisplacementRestrictionStates.Add(new NodeDisplacementRestrictionState
{
    NodeId = 1,
    MaxDisplacement = 0.5722201688825613,
    Axis = Axis.X,
});


// commandManager.Push(new AddNodeCommand(construction, Vector2.Zero));
// commandManager.Push(new AddNodeCommand(construction, new Vector2(1000, 0)));
// commandManager.Push(new AddNodeCommand(construction, new Vector2(1000, 1000)));
//
// commandManager.Push(new AddLoadCommand(construction, 2, new Vector2(0, 1000)));
//
// commandManager.Push(new AddConstraintCommand(construction, 0, false, true));
// commandManager.Push(new AddConstraintCommand(construction, 1, true, true));
//
//
// commandManager.Push(new AddElementCommand(construction, 0, 1, 2_000_000, 14));
// commandManager.Push(new AddElementCommand(construction, 1, 2, 2_000_000, 14));
// commandManager.Push(new AddElementCommand(construction, 2, 0, 2_000_000, 14));

#endif
var editor = serviceProvider.GetRequiredService<Editor>();
editor.Run();

return;

ServiceProvider ConfigureServices()
{
    var services = new ServiceCollection()
        .AddSingleton<AxisRender>()
        .AddSingleton<CalculationResults>()
        .AddSingleton<CameraController>()
        .AddSingleton<CommandManager>()
        .AddSingleton<ConstructionController>()
        .AddSingleton<Editor>()
        .AddSingleton<EditorConsole>()
        .AddSingleton<EditorConstruction>()
        .AddSingleton<ExplorerController>()
        .AddSingleton<ICommandManager>(provider => provider.GetRequiredService<CommandManager>())
        .AddSingleton<IMenu, ConstructionMenu>()
        .AddSingleton<IMenu, DiagnosticsMenu>()
        .AddSingleton<ConstructionRestrictionsWindow>()
        .AddSingleton<DiagnosticsWindow>()
        .AddSingleton<RestrictionsState>()
        .AddSingleton<MenuBar>()
        .AddSingleton<QueueManager>()
        .AddSingleton<SceneBuffer>()
        .AddSingleton<SceneController>()
        .AddSingleton<ConstructionManager>()
        .AddQueueManagedServices()
        .AddCoreServices();


    return services.BuildServiceProvider();
}
