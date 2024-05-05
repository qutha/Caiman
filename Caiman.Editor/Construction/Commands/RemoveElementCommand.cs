using Caiman.Editor.Commands;

namespace Caiman.Editor.Construction.Commands;

public class RemoveElementCommand : ICommand
{
    private readonly EditorConstruction _construction;
    private readonly int _elementId;
    private EditorElement? _addedElement;

    public RemoveElementCommand(EditorConstruction construction, int elementId)
    {
        _construction = construction;
        _elementId = elementId;
    }

    #region ICommand Members

    public string Name => "Remove element";

    public void Execute() => _addedElement = _construction.RemoveElement(_elementId);

    public void Undo()
    {
        if (_addedElement is null)
        {
            return;
        }

        _construction.AddDeletedElement(_addedElement);
    }

    #endregion
}
