using Caiman.Core.Depr.Construction;
using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class AddElementCommand : ICommand
{
    private readonly double _area;
    private readonly ConstructionModel _constructionModel;
    private readonly double _elasticity;
    private readonly int _endNodeId;
    private readonly int _startNodeId;
    private int _addedElementId;

    public AddElementCommand(
        ConstructionModel constructionModel,
        int startNodeId,
        int endNodeId,
        double elasticity,
        double area)
    {
        _constructionModel = constructionModel;
        _startNodeId = startNodeId;
        _endNodeId = endNodeId;
        _elasticity = elasticity;
        _area = area;
    }

    public string Name => "Add element";

    public void Execute() =>
        _addedElementId = _constructionModel.AddElement(
            _startNodeId, _endNodeId, _elasticity, _area);

    public void Undo() => _constructionModel.RemoveElement(_addedElementId);
}
