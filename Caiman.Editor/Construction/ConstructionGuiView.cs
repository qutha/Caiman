using System.Diagnostics;
using System.Numerics;
using Caiman.Editor.Interfaces;
using ImGuiNET;

namespace Caiman.Editor.Construction;

public class ConstructionGuiView : IGuiRender
{
    private readonly ConstraintMenuState _constraintMenuState = new();
    private readonly EditorConstruction _construction;
    private readonly ElementMenuState _elementMenuState = new();
    private readonly LoadMenuState _loadMenuState = new();
    private readonly NodeMenuState _nodeMenuState = new();

    public ConstructionGuiView(EditorConstruction construction) => _construction = construction;

    #region IGuiRender Members

    public void RenderGui()
    {
        ImGui.Begin("Tools");

        if (ImGui.BeginTabBar("Tabs"))
        {
            if (ImGui.BeginTabItem("Node"))
            {
                RenderNodeMenu();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Element"))
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

    #endregion

    #region Events

    public event Action<Vector2>? AddNode;
    public event Action<int>? RemoveNode;

    public event Action<(int startNodeId, int endNodeId, double elasticity, double area)>? AddElement;

    public event Action<int>? RemoveElement;

    public event Action<int, Vector2>? AddLoad;
    public event Action<int, int>? RemoveLoad;
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
        public int LoadId;
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
        if (ImGui.Button("Add Node"))
        {
            AddNode?.Invoke(new Vector2(_nodeMenuState.X, _nodeMenuState.Y));
        }

        ImGui.Separator();
        ImGui.InputInt("Id", ref _nodeMenuState.Id);
        if (ImGui.Button("Remove Node"))
        {
            if (_nodeMenuState.Id < 0 || _nodeMenuState.Id >= _construction.Nodes.Count)
            {
                Debug.WriteLine("Invalid Id");
                return;
            }

            RemoveNode?.Invoke(_nodeMenuState.Id);
        }
    }

    private void RenderElementMenu()
    {
        ImGui.InputInt("Start Node Id", ref _elementMenuState.StartNodeId);
        ImGui.InputInt("End Node Id", ref _elementMenuState.EndNodeId);
        ImGui.InputDouble("Elasticity", ref _elementMenuState.Elasticity, 10, 400);
        ImGui.InputDouble("Area", ref _elementMenuState.Area, 0.1, 10);
        if (ImGui.Button("Add element"))
        {
            AddElement?.Invoke((_elementMenuState.StartNodeId, _elementMenuState.EndNodeId,
                _elementMenuState.Elasticity,
                _elementMenuState.Area));
        }

        ImGui.Separator();
        ImGui.InputInt("Id", ref _elementMenuState.Id);
        if (ImGui.Button("Remove Element"))
        {
            RemoveElement?.Invoke(_elementMenuState.Id);
        }
    }

    private void RenderConstraintMenu()
    {
        ImGui.InputInt("Node Id", ref _constraintMenuState.NodeId);
        ImGui.Checkbox("X", ref _constraintMenuState.X);
        ImGui.SameLine();
        ImGui.Checkbox("Y", ref _constraintMenuState.Y);
        if (ImGui.Button("Add Constraint"))
        {
            AddConstraint?.Invoke(_constraintMenuState.NodeId, _constraintMenuState.X, _constraintMenuState.Y);
        }

        if (ImGui.Button("Remove Constraint"))
        {
            RemoveConstraint?.Invoke(_constraintMenuState.NodeId);
        }
    }

    private void RenderLoadMenu()
    {
        ImGui.InputFloat("X", ref _loadMenuState.X, 0, 100);
        ImGui.InputFloat("Y", ref _loadMenuState.Y, 0, 100);
        ImGui.InputInt("Node Id", ref _loadMenuState.NodeId);
        if (ImGui.Button("Add Load"))
        {
            AddLoad?.Invoke(_loadMenuState.NodeId, new Vector2(_loadMenuState.X, _loadMenuState.Y));
        }

        ImGui.Separator();
        ImGui.InputInt("Load Id", ref _loadMenuState.LoadId);
        if (ImGui.Button("Remove Load"))
        {
            RemoveLoad?.Invoke(_loadMenuState.NodeId, _loadMenuState.LoadId);
        }
    }

    #endregion Menu
}
