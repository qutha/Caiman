using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class AddElementCommand : ICommand
{
    private readonly double _area;
    private readonly EditorConstruction _construction;
    private readonly double _elasticity;
    private readonly int _endNodeId;
    private readonly int _startNodeId;
    private int _addedElementId;

    public AddElementCommand(
        EditorConstruction construction,
        int startNodeId,
        int endNodeId,
        double elasticity,
        double area)
    {
        _construction = construction;
        _startNodeId = startNodeId;
        _endNodeId = endNodeId;
        _elasticity = elasticity;
        _area = area;
    }

    #region ICommand Members

    public string Name => "Add Element";

    public void Execute() =>
        _addedElementId = _construction.AddElement(
            _startNodeId, _endNodeId, _elasticity, _area);

    public void Undo() => _construction.RemoveElement(_addedElementId);

    #endregion
}
