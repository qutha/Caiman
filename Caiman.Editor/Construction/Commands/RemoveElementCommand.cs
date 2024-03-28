using Caiman.Core.Depr.Construction;
using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class RemoveElementCommand : ICommand
{
    private readonly ConstructionModel _constructionModel;
    private readonly int _elementId;
    private Element? _addedElement;

    public RemoveElementCommand(ConstructionModel constructionModel, int elementId)
    {
        _constructionModel = constructionModel;
        _elementId = elementId;
    }

    public string Name => "Remove element";

    public void Execute() => _addedElement = _constructionModel.RemoveElement(_elementId);

    public void Undo()
    {
        if (_addedElement is null)
        {
            return;
        }

        _constructionModel.AddDeletedElement(_addedElement);
    }
}
