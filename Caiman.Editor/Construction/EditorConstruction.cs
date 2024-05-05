using System.Diagnostics;
using System.Numerics;
using Caiman.Core;
using Caiman.Core.Construction;
using Caiman.Core.Construction.Exceptions;
using Caiman.Editor.Interfaces;
using Raylib_cs;

namespace Caiman.Editor.Construction;

public class EditorConstruction : IWorldRender
{
    private readonly Color _freeConstraint = Color.Green;
    private readonly Color _uConstraint = Color.Yellow;
    private readonly Color _uvConstraint = Color.Red;
    private readonly Color _vConstraint = Color.Yellow;
    private float _zoom = 1;
    public List<EditorElement> Elements { get; set; } = [];

    public List<EditorNode> Nodes { get; set; } = [];
    private int Distance => (int)(40 / _zoom);
    private int FontSize => (int)(20 / _zoom);
    private float Thickness => 2 / _zoom;

    private float LoadLength => 50 / _zoom;
    private float ArrowAngle => 20;
    private float ArrowLength => 15 / _zoom;
    private float ArrowThickness => 2 / _zoom;
    private Color ArrowColor => Color.Blue;

    private float Radius => 5 / _zoom;

    #region IWorldRender Members

    public void RenderWorld()
    {
        foreach (var element in Elements)
        {
            var startNode = Nodes[element.StartNode.Id];
            var endNode = Nodes[element.EndNode.Id];
            var startPos = new Vector2(startNode.X, startNode.Y);
            var endPos = new Vector2(endNode.X, endNode.Y);
            Raylib.DrawLineEx(startPos, endPos, Thickness, Color.Red);

            var midPoint = new Vector2((startPos.X + endPos.X) / 2,
                (startPos.Y + endPos.Y) / 2);
            var direction = Vector2.Subtract(endPos, startPos);
            var angle = (float)Math.Atan2(direction.Y, direction.X);
            var textPosition = CalculateTextPosition(midPoint, angle, Distance);

            Raylib.DrawText(element.Id.ToString(), (int)textPosition.X, (int)textPosition.Y, FontSize, Color.Red);
        }

        foreach (var node in Nodes)
        {
            var color = node.Constraint switch
            {
                { X: true, Y: true } => _uvConstraint,
                { X: false, Y: true } => _vConstraint,
                { X: true, Y: false } => _uConstraint,
                _ => _freeConstraint,
            };

            if (node.Loads.Count != 0)
            {
                var load = node.Loads.Aggregate(new Vector2(0, 0), (vector, load) =>
                {
                    vector.X += (float)load.X;
                    vector.Y -= (float)load.Y;
                    return vector;
                });
                var loadLength = load.Length();
                load.X = load.X / loadLength * LoadLength;
                load.Y = load.Y / loadLength * LoadLength;
                var startPoint = new Vector2(node.X, node.Y);
                var endPoint = startPoint + load;
                DrawArrow(
                    startPoint,
                    endPoint,
                    ArrowLength,
                    ArrowAngle,
                    ArrowThickness,
                    ArrowColor
                );
            }

            Raylib.DrawCircleV(new Vector2(node.X, node.Y), Radius, color);
            Raylib.DrawText($"{node.Id}", (int)node.X, (int)node.Y - Distance, FontSize, color);
        }
    }

    #endregion

    private void DrawArrow(
        Vector2 startPoint,
        Vector2 endPoint,
        float wingLength,
        float wingAngle,
        float thickness,
        Color color
    )
    {
        // Рисуем основную линию стрелки
        Raylib.DrawLineEx(startPoint, endPoint, thickness, color);

        // Вычисляем угол между основной линией и осью X
        var angle = (float)(Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X) * 180 / Math.PI);

        // Вычисляем координаты крыльев
        var wing1 = CalculateWing(endPoint, wingLength, wingAngle, angle);
        var wing2 = CalculateWing(endPoint, wingLength, -wingAngle, angle);

        // Рисуем крылья стрелки
        Raylib.DrawLineEx(endPoint, wing1, thickness, color);
        Raylib.DrawLineEx(endPoint, wing2, thickness, color);
    }

    // Функция для вычисления координаты крыла стрелки
    private Vector2 CalculateWing(Vector2 endPoint, float wingLength, float angle, float baseAngle)
    {
        // Преобразование угла из градусов в радианы
        var angleRadians = (baseAngle + angle) * (float)(Math.PI / 180);

        // Вычисляем смещение для крыла
        var offsetX = wingLength * (float)Math.Cos(angleRadians);
        var offsetY = wingLength * (float)Math.Sin(angleRadians);

        // Вычисляем координаты крыла
        var wingX = endPoint.X - offsetX;
        var wingY = endPoint.Y - offsetY;

        return new Vector2(wingX, wingY);
    }


    public void AddConstraint(int nodeId, bool x, bool y)
    {
        var node = Nodes.Find(n => n.Id == nodeId) ?? throw new NodeNotFoundException();
        node.Constraint = new Constraint(x, y);
    }

    public void AddDeletedElement(EditorElement element, bool wasInNode = false)
    {
        Elements.Insert(element.Id, element);
        if (!wasInNode)
        {
            NumberElements();
        }
    }

    public void AddDeletedNode(EditorNode node)
    {
        Nodes.Insert(node.Id, node);
        foreach (var element in node.Elements)
        {
            AddDeletedElement(element, true);
        }

        if (node.Elements.Count > 0)
        {
            NumberElements();
        }

        NumberNodes();
    }

    public int AddElement(int startNodeId, int endNodeId, double elasticity, double area)
    {
        if (startNodeId < 0 || startNodeId >= Nodes.Count
            || endNodeId < 0 || endNodeId >=
            Nodes.Count)
        {
            Debug.WriteLine("Invalid Id");
            throw new NodeNotFoundException();
        }

        if (startNodeId == endNodeId)
        {
            Debug.WriteLine("Start and end nodes are the same");
            throw new SameNodesException();
        }

        var startNode = Nodes[startNodeId];
        var endNode = Nodes[endNodeId];

        var element = Elements.Find(el =>
            (el.StartNode.Id == startNode.Id || el.StartNode.Id == endNode.Id) && (el.EndNode.Id == startNode.Id ||
                el.EndNode.Id == endNode.Id)
        );
        if (element is not null)
        {
            Debug.WriteLine("Element already exists");
            throw new ElementAlreadyExistsException();
        }

        var newElement = new EditorElement(
            startNode,
            endNode,
            elasticity,
            area
        );
        Elements.Add(
            newElement
        );
        startNode.Elements.Add(newElement);
        endNode.Elements.Add(newElement);
        NumberElements();
        return newElement.Id;
    }

    public void AddLoad(int nodeId, Vector2 value)
    {
        if (value == Vector2.Zero)
        {
            throw new ZeroLoadException();
        }

        var node = Nodes.Find(n => n.Id == nodeId) ?? throw new NodeNotFoundException();
        node.Loads.Add(new ConcentratedLoad(value.X, value.Y));
    }

    public int AddNode(Vector2 position)
    {
        var node = Nodes.Find(node =>
            Math.Abs(node.X - position.X) < Constants.Epsilon &&
            Math.Abs(node.Y - position.Y) < Constants.Epsilon);
        if (node is not null)
        {
            throw new NodeAlreadyExistsException();
        }

        var newNode = new EditorNode(position);
        Nodes.Add(
            newNode
        );
        NumberNodes();
        return newNode.Id;
    }

    public Constraint RemoveConstraint(int nodeId)
    {
        var node = Nodes.Find(n => n.Id == nodeId) ?? throw new NodeNotFoundException();
        var constraint = node.Constraint;
        node.Constraint = new Constraint(false, false);
        return constraint;
    }

    public EditorElement RemoveElement(int elementId)
    {
        try
        {
            var element = Elements[elementId];
            _ = Elements.Remove(element);
            _ = element.StartNode.Elements.Remove(element);
            _ = element.EndNode.Elements.Remove(element);
            NumberElements();
            return element;
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new ElementNotFoundException();
        }
    }

    public ConcentratedLoad RemoveLoad(int nodeId, int loadId)
    {
        var node = Nodes.Find(n => n.Id == nodeId) ?? throw new NodeNotFoundException();

        if (loadId < 0 && loadId >= node.Loads.Count)
        {
            throw new LoadNotFoundException();
        }

        var load = node.Loads[loadId];
        node.Loads.RemoveAt(loadId);
        return load;
    }

    public void RemoveLoad(int nodeId, Vector2 value)
    {
        var node = Nodes.Find(n => n.Id == nodeId) ?? throw new NodeNotFoundException();
        var load = node.Loads.Find(c =>
                Math.Abs(c.X - value.X) < Constants.Epsilon && Math.Abs(c.Y - value.Y) < Constants.Epsilon) ??
            throw new LoadNotFoundException();
        _ = node.Loads.Remove(load);
    }

    public EditorNode RemoveNode(int nodeId)
    {
        // TODO переделать на FirstOrDefault
        try
        {
            var node = Nodes[nodeId];
            _ = Nodes.Remove(node);
            var removedElements = RemoveElements(node);
            if (removedElements > 0)
            {
                NumberElements();
            }

            NumberNodes();
            return node;
        }
        catch (IndexOutOfRangeException)
        {
            throw new NodeNotFoundException();
        }
    }

    private void NumberElements()
    {
        foreach (var element in Elements)
        {
            element.Id = Elements.IndexOf(element);
        }
    }

    private void NumberNodes()
    {
        foreach (var node in Nodes)
        {
            node.Id = Nodes.IndexOf(node);
        }
    }

    private int RemoveElements(EditorNode node) =>
        Elements.RemoveAll(element => element.StartNode == node || element.EndNode == node);


    private Vector2 CalculateTextPosition(Vector2 midPoint, float angle, float distance)
    {
        var textX = midPoint.X + distance * (float)Math.Cos(angle - Math.PI / 2);
        var textY = midPoint.Y + distance * (float)Math.Sin(angle - Math.PI / 2);
        return new Vector2(textX, textY);
    }


    public void ChangeZoom(float zoom) => _zoom = zoom;
}
