using System.Numerics;
using Caiman.Core.Depr.Construction;
using Caiman.Editor.Interfaces;
using Raylib_cs;

namespace Caiman.Editor.Construction;

public class ConstructionGraphicsView(IConstructionModel constructionModel) : IWorldRender
{
    public void RenderWorld()
    {
        Raylib.DrawLineEx(new Vector2(-100, 0), new Vector2(100, 0), 2, Color.Red);
        Raylib.DrawLineEx(new Vector2(0, -100), new Vector2(0, 100), 2, Color.Red);
        const int distance = 40;
        const int fontSize = 20;
        foreach (Element element in constructionModel.Elements)
        {
            Node startNode = constructionModel.Nodes[element.StartNode.Id];
            Node endNode = constructionModel.Nodes[element.EndNode.Id];
            Vector2 startPos = startNode.Position with { Y = -startNode.Position.Y };
            Vector2 endPos = endNode.Position with { Y = -endNode.Position.Y };
            Raylib.DrawLineEx(startPos, endPos, 5, Color.Red);

            var midPoint = new Vector2((startPos.X + endPos.X) / 2,
                (startPos.Y + endPos.Y) / 2);
            Vector2 direction = Vector2.Subtract(endPos, startPos);
            var angle = (float)Math.Atan2(direction.Y, direction.X);
            Vector2 textPosition = CalculateTextPosition(midPoint, angle, distance);

            Raylib.DrawText($"{element.Id}", (int)textPosition.X, (int)textPosition.Y, fontSize, Color.Red);
        }

        foreach (Node node in constructionModel.Nodes)
        {
            Vector2 pos = node.Position with { Y = -node.Position.Y };
            Raylib.DrawCircleV(pos, 10, Color.Green);
            Raylib.DrawText($"{node.Id}", (int)pos.X, (int)pos.Y - distance, fontSize, Color.Green);
            if (node.Constraint is not null)
            {
                RenderConstraint(node);
            }
        }
    }

    private Vector2 CalculateTextPosition(Vector2 midPoint, float angle, float distance)
    {
        float textX = midPoint.X + distance * (float)Math.Cos(angle - Math.PI / 2);
        float textY = midPoint.Y + distance * (float)Math.Sin(angle - Math.PI / 2);
        return new Vector2(textX, textY);
    }

    private void RenderConstraint(Node node)
    {
        const double length = 25;
        if (node.Constraint!.X)
        {
            Raylib.DrawLineEx(node.Position, node.Position with { X = (float)(node.Position.X + length) }, 2,
                Color.Blue);
        }

        if (node.Constraint!.Y)
        {
            Raylib.DrawLineEx(node.Position, node.Position with { Y = (float)(node.Position.Y + length) }, 2,
                Color.Blue);
        }
    }
}
