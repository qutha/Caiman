using System.Diagnostics;
using System.Numerics;
using Caiman.Core.Depr.Construction;
using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor.Construction;

public class ConstructionGuiView : IGuiRender
{
    private readonly ConstraintMenuState _constraintMenuState = new();
    private readonly ConstructionModel _constructionModel;
    private readonly ElementMenuState _elementMenuState = new();
    private readonly LoadMenuState _loadMenuState = new();
    private readonly NodeMenuState _nodeMenuState = new();

    public ConstructionGuiView(ConstructionModel constructionModel) => _constructionModel = constructionModel;

    public void RenderGui()
    {
        ImGui.Begin("Tools");

        if (ImGui.BeginTabBar("Tabs"))
        {
            if (ImGui.BeginTabItem("NodeEntity"))
            {
                RenderNodeMenu();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("ElementEntity"))
            {
                RenderElementMenu();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Constraint"))
            {
                RenderConstraintMenu();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Load"))
            {
                RenderLoadMenu();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }

        ImGui.End();
    }

    #region Events

    public event Action<Vector2>? AddNode;
    public event Action<int>? RemoveNode;

    public event Action<(int startNodeId, int endNodeId, double elasticity, double area)>? AddElement;

    public event Action<int>? RemoveElement;

    public event Action<int, Vector2>? AddLoad;
    public event Action<int, Vector2>? RemoveLoad;
    public event Action<int, bool, bool>? AddConstraint;
    public event Action<int>? RemoveConstraint;

    #endregion


    #region States

    private class ConstraintMenuState
    {
        public int NodeId;
        public bool X;
        public bool Y;
    }

    private class LoadMenuState
    {
        public int NodeId;
        public float X;
        public float Y;
    }

    private class NodeMenuState
    {
        public int Id;
        public float X;
        public float Y;
    }

    private class ElementMenuState
    {
        public double Area;
        public double Elasticity;
        public int EndNodeId;
        public int Id;
        public int StartNodeId;
    }

    #endregion


    #region Menu

    private void RenderNodeMenu()
    {
        ImGui.InputFloat("X", ref _nodeMenuState.X);
        ImGui.InputFloat("Y", ref _nodeMenuState.Y);
        if (ImGui.Button("Add NodeEntity"))
        {
            AddNode?.Invoke(new Vector2(_nodeMenuState.X, _nodeMenuState.Y));
        }

        ImGui.InputInt("Id", ref _nodeMenuState.Id);
        if (ImGui.Button("Remove NodeEntity"))
        {
            if (_nodeMenuState.Id < 0 || _nodeMenuState.Id >= _constructionModel.Nodes.Count)
            {
                Debug.WriteLine("Invalid Id");
                return;
            }

            RemoveNode?.Invoke(_nodeMenuState.Id);
        }
    }

    private void RenderElementMenu()
    {
        ImGui.InputInt("Start NodeEntity Id", ref _elementMenuState.StartNodeId);
        ImGui.InputInt("End NodeEntity Id", ref _elementMenuState.EndNodeId);
        ImGui.InputDouble("Elasticity", ref _elementMenuState.Elasticity, 10, 400);
        ImGui.InputDouble("Area", ref _elementMenuState.Area, 0.1, 10);
        if (ImGui.Button("Add element"))
        {
            AddElement?.Invoke((_elementMenuState.StartNodeId, _elementMenuState.EndNodeId,
                _elementMenuState.Elasticity,
                _elementMenuState.Area));
        }

        ImGui.InputInt("Id", ref _elementMenuState.Id);
        if (ImGui.Button("Remove ElementEntity"))
        {
            RemoveElement?.Invoke(_elementMenuState.Id);
        }
    }

    private void RenderConstraintMenu()
    {
        ImGui.InputInt("Start NodeEntity Id", ref _constraintMenuState.NodeId);
        ImGui.Checkbox("X", ref _constraintMenuState.X);
        ImGui.Checkbox("Y", ref _constraintMenuState.Y);
        if (ImGui.Button("Add Constraint"))
        {
            AddConstraint?.Invoke(_constraintMenuState.NodeId, _constraintMenuState.X, _constraintMenuState.Y);
        }

        ImGui.InputInt("NodeEntity Id", ref _constraintMenuState.NodeId);
        if (ImGui.Button("Remove Constraint"))
        {
            RemoveConstraint?.Invoke(_constraintMenuState.NodeId);
        }
    }

    private void RenderLoadMenu()
    {
        ImGui.InputFloat("X", ref _loadMenuState.X, 0, 100);
        ImGui.InputFloat("Y", ref _loadMenuState.Y, 0, 100);
        ImGui.InputInt("NodeEntity Id", ref _loadMenuState.NodeId);
        if (ImGui.Button("Add Load"))
        {
            AddLoad?.Invoke(_loadMenuState.NodeId, new Vector2(_loadMenuState.X, _loadMenuState.Y));
        }

        if (ImGui.Button("Remove Load"))
        {
            RemoveLoad?.Invoke(_loadMenuState.NodeId, new Vector2(_loadMenuState.X, _loadMenuState.Y));
        }
    }

    #endregion Menu
}
