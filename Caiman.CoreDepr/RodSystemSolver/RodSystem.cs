namespace Caiman.CoreDepr.RodSystemSolver;

public class RodSystem
{
    private readonly List<NodeReference> _nodeReferences = new();
    private bool _isBuilt;

    public List<NodeReference> NodeReferences
    {
        get
        {
            if (!_isBuilt)
            {
                throw new InvalidOperationException("RodSystem is not built");
            }

            return _nodeReferences;
        }
    }

    public List<Element> Elements { get; } = new();


    public RodSystem AddElement(Element element)
    {
        NodeReference? startNodeReference = _nodeReferences.Find(n => n.Node.Equals(element.StartNode));
        if (startNodeReference == null)
        {
            startNodeReference = new NodeReference(element.StartNode);
            _nodeReferences.Add(startNodeReference);
        }

        startNodeReference.Elements.Add(element);


        NodeReference? endNodeReference = _nodeReferences.Find(n => n.Node.Equals(element.EndNode));
        if (endNodeReference == null)
        {
            endNodeReference = new NodeReference(element.EndNode);
            _nodeReferences.Add(endNodeReference);
        }

        endNodeReference.Elements.Add(element);
        Elements.Add(element);
        SetNeedBuild();
        return this;
    }

    private void SetNeedBuild() => _isBuilt = false;

    public void Build()
    {
        NumberNodes();
        _isBuilt = true;
    }

    private void NumberNodes() =>
        _nodeReferences.ForEach(n =>
        {
            n.Node.Id = _nodeReferences.IndexOf(n);
            n.Id = _nodeReferences.IndexOf(n);
        });

    public RodSystem RemoveElement(Element element)
    {
        NodeReference? startNodeReference = _nodeReferences.Find(n => n.Node.Equals(element.StartNode));
        startNodeReference?.Elements.Remove(element);
        if (startNodeReference != null)
        {
            TryRemoveNodeReference(startNodeReference);
        }


        NodeReference? endNodeReference = _nodeReferences.Find(n => n.Node.Equals(element.EndNode));
        endNodeReference?.Elements.Remove(element);
        if (endNodeReference != null)
        {
            TryRemoveNodeReference(endNodeReference);
        }

        Elements.Remove(element);
        SetNeedBuild();
        return this;
    }

    private void TryRemoveNodeReference(NodeReference nodeReference)
    {
        // TODO проверить, существует ли элемент
        if (nodeReference.Elements.Count == 0 && nodeReference.Loads.Count == 0 && nodeReference.Constraint == null)
        {
            _nodeReferences.Remove(nodeReference);
        }
    }

    public RodSystem AddLoad(ConcentratedLoad concentratedLoad, Node node)
    {
        NodeReference? nodeReference = _nodeReferences.Find(n => n.Node.Equals(node));
        nodeReference?.Loads.Add(concentratedLoad);
        return this;
    }

    public RodSystem RemoveLoad(ConcentratedLoad concentratedLoad)
    {
        NodeReference? nodeReference = _nodeReferences.Find(n => n.Loads.Contains(concentratedLoad));
        nodeReference?.Loads.Remove(concentratedLoad);
        if (nodeReference != null)
        {
            TryRemoveNodeReference(nodeReference);
        }

        return this;
    }

    public RodSystem AddConstraint(Constraint constraint, Node node)
    {
        NodeReference? nodeReference = _nodeReferences.Find(n => n.Node.Equals(node));
        if (nodeReference != null)
        {
            nodeReference.Constraint = constraint;
        }

        return this;
    }

    public RodSystem RemoveConstraint(Constraint constraint)
    {
        NodeReference? nodeReference = _nodeReferences.Find(n => n.Constraint == constraint);
        if (nodeReference == null)
        {
            return this;
        }

        nodeReference.Constraint = null;
        TryRemoveNodeReference(nodeReference);

        return this;
    }
}
