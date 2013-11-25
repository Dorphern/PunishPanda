using System;
using InAudio;
using InAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace InAudio
{

public class TreeDrawer<T> where T : UnityEngine.Object, ITreeNode<T>
{
    public T SelectedNode
    {
        get { return selectedNode; }
        set { selectedNode = value; }
    }

    private T selectedNode;
    private Rect selectedArea;

    //private bool dirty = false;
    public bool IsDirty
    {
        get;
        private set;
    }

    public delegate void OnContextDelegate(T node);
    public OnContextDelegate OnContext;

    public delegate bool OnNodeDrawDelegate(T node, bool isSelected);
    public OnNodeDrawDelegate OnNodeDraw;

    public delegate void OnDropDelegate(T node, UnityEngine.Object[] objects);
    public OnDropDelegate OnDrop;

    public delegate bool CanDropObjectsDelegate(T node, UnityEngine.Object[] objects);
    public CanDropObjectsDelegate CanDropObjects;

    private T HoverOver;
    private Rect HoverOverArea;

    private bool triggerFilter = false;
    private Func<T, bool> filterFunc;


    private bool canDropObjects;
    private Vector2 dragStart;
    private bool wantToDrag;
    private bool dragging;

    private Rect _area;

    public Vector2 ScrollPosition;

    public void SelectPreviousNode()
    {
        selectedNode = TreeWalker.FindPreviousUnfoldedNode(SelectedNode);
    }

    public void SelectNextNode()
    {
        selectedNode = TreeWalker.FindNextUnfoldedNode(SelectedNode);
    }

    public bool DrawTree(T root, Rect area)
    {
        ScrollPosition = EditorGUILayout.BeginScrollView(ScrollPosition, false, true);
        if (root == null || OnNodeDraw == null)
            return true;

        _area = area;

        if (triggerFilter)
        {
            FilterNodes(root, filterFunc);
            triggerFilter = false;
            IsDirty = true;
        }


        IsDirty = false;

        bool canDropObject = false;
        Vector2 mousePos = Event.current.mousePosition;
        if (mousePos.x > area.x && mousePos.x < area.x + area.width)
        {
            canDropObject = HandleDragging();
        }

        DrawTree(root, EditorGUI.indentLevel);

        PostDrawDragHandle(canDropObject);

        ContextHandle();

        KeyboardControl();


        EditorGUILayout.EndScrollView();

        return IsDirty;
    }

    //Draw all nodes recursively 
    void DrawTree(T node, int indentLevel)
    {
        if (node != null)
        {
            if (node.IsFiltered)
                return;
            EditorGUI.indentLevel = indentLevel + 1;
            DrawNode(node);
            EditorGUI.indentLevel = indentLevel - 1;

            if (!node.IsFoldedOut)
                return;

            for (int i = 0; i < node.GetChildren.Count; ++i)
            {
                T child = node.GetChildren[i];
                DrawTree(child, indentLevel + 1);
            }
        }
    }

    private void ContextHandle()
    {
        if (Event.current.type == EventType.ContextClick && selectedArea.Contains(Event.current.mousePosition) && OnContext != null)
        {
            OnContext(selectedNode);

            Event.current.Use();
        }
    }

    private void KeyboardControl()
    {
        #region keyboard control

        bool hasPressedDown = false;
        bool hasPressedUp = false;
        if (Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.RightArrow)
        {
            SelectedNode.IsFoldedOut = true;
            Event.current.Use();
        }
        if (Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.LeftArrow)
        {
            SelectedNode.IsFoldedOut = false;
            Event.current.Use();
        }

        if (Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.UpArrow)
        {
            hasPressedUp = true;
            selectedNode = TreeWalker.FindPreviousUnfoldedNode(SelectedNode);
            Event.current.Use();
        }
        if (Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.DownArrow)
        {
            hasPressedDown = true;
            selectedNode = TreeWalker.FindNextUnfoldedNode(SelectedNode);
            Event.current.Use();
        }

        if (hasPressedDown && (_area.y + ScrollPosition.y + _area.height - selectedArea.height * 2 < selectedArea.y + selectedArea.height))
        {
            ScrollPosition.y += selectedArea.height;
        }

        if (hasPressedUp && (_area.y + ScrollPosition.y + selectedArea.height  > selectedArea.y))
        {
            ScrollPosition.y -= selectedArea.height;
        }
        #endregion
    }

    private void DrawNode(T child)
    {
        
        bool previousState = child.IsFoldedOut;

        
        child.IsFoldedOut = OnNodeDraw(child, child == selectedNode);

        Rect lastArea = GUILayoutUtility.GetLastRect();
        

        if (lastArea.Contains(Event.current.mousePosition))
        {
            HoverOver = child;
            HoverOverArea = lastArea;
        }

        if (CheckIsSelected(lastArea)  || (child == selectedNode || child.IsFoldedOut != previousState))
        {
            if (selectedNode != child)
                IsDirty = true;
            selectedNode = child;
            AssignSelectedArea(lastArea);
        }
    }

    private void DrawBackground(Rect area)
    {
        GUI.DrawTexture(area, EditorResources.Background);
    }

    private bool CheckIsSelected(Rect area)
    {
        return (Event.current.type == EventType.MouseDown || Event.current.type == EventType.ContextClick) && area.Contains(Event.current.mousePosition) && Event.current.type != EventType.Repaint;
    }

    private void AssignSelectedArea(Rect area)
    {
        selectedArea = area;
        selectedArea.width += 15;
    }

    //FilterBy: true if node contains search
    private bool FilterNodes(T node, Func<T, bool> filter)
    {
        node.IsFiltered = false;
        if (node.GetChildren.Count > 0)
        {
            bool allChildrenFilted = true;
            foreach (var child in node.GetChildren)
            {
                bool filtered = FilterNodes(child, filter);
                if (!filtered)
                {
                    allChildrenFilted = false;
                }
            }
            node.IsFiltered = allChildrenFilted; //If all children are filtered, this node also becomes filtered unless its name is not filtered
            if(node.IsFiltered)
                node.IsFiltered = filter(node);
            return node.IsFiltered;
        }
        else
        {
            node.IsFiltered = filter(node);
            return node.IsFiltered;
        }
    }

    public void Filter(Func<T, bool> filter)
    {
        filterFunc = filter;
        triggerFilter = true;
    }

    private bool HandleDragging()
    { 
        if (Event.current.ClickedWithin(_area) && Event.current.button == 0)
        {
            dragStart = Event.current.mousePosition;
            wantToDrag = true;
        }

        if (wantToDrag && !dragging && Event.current.type == EventType.MouseDrag && Vector2.Distance(dragStart, Event.current.mousePosition) > 10.0f)
        {
            wantToDrag = false;
            dragging = true;
            if (selectedNode != null)
            {
                DragAndDrop.PrepareStartDrag(); 
                DragAndDrop.SetGenericData(selectedNode.GetName, selectedNode);
                DragAndDrop.paths = null;
                DragAndDrop.objectReferences = new UnityEngine.Object[] {selectedNode};
                DragAndDrop.StartDrag("AudioNode");
            }
            Event.current.Use();
        }

        if (Event.current.type != EventType.MouseDrag)
        {
            dragging = false;
        }

        bool canDropObjects = false;
        if (CanDropObjects != null)
            canDropObjects = CanDropObjects(HoverOver, DragAndDrop.objectReferences);
        if (canDropObjects)
        {
            DrawBackground(HoverOverArea);
            IsDirty = true;
        }
        return canDropObjects;
    }

    private void PostDrawDragHandle(bool canDropObject)
    {
        if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
        {
            if (canDropObject)
            {
                if (HoverOver != null)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

                    if (Event.current.type == EventType.DragPerform)
                    {
                        OnDrop(HoverOver, DragAndDrop.objectReferences);
                    }
                }
            }
            else
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.None;
            }
        }
    }
}
}