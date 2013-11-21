using System;
using InAudio;
using InAudio.HDREditorGUI;
using InAudio.TreeDrawer;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class AudioBusCreatorGUI : BaseCreatorGUI<AudioBus>
{
    public AudioBusCreatorGUI(AuxWindow window)
        : base(window)
    {
        this.window = window;
    }

    private int leftWidth;
    private int height;

    public bool OnGUI(int leftWidth, int height)
    {
        BaseOnGUI();

        var root = HDRInstanceFinder.DataManager.BusTree;
        int id = HDRInstanceFinder.InAudioGuiUserPrefs.SelectedBusID;
        var selectedNode = UpdateSelectedNode(root, id);
        HDRInstanceFinder.InAudioGuiUserPrefs.SelectedBusID = selectedNode != null ? selectedNode.ID : 0;
    

        this.leftWidth = leftWidth;
        this.height = height;

        EditorGUIHelper.DrawColums(DrawLeftSide, DrawRightSide);

        return isDirty;
    }

    private void DrawLeftSide(Rect area)
    {
        Rect treeArea = EditorGUILayout.BeginVertical(GUILayout.Width(leftWidth), GUILayout.Height(height));
        DrawSearchBar();

        EditorGUILayout.BeginVertical();
        treeArea.y -= 25;
        //treeArea.height += 10;
        isDirty |= treeDrawer.DrawTree(HDRInstanceFinder.DataManager.BusTree, treeArea);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
    }

    private void DrawRightSide(Rect area)
    {
        if (treeDrawer.SelectedNode != null)
        {
            AudioBusDrawer.Draw(treeDrawer.SelectedNode);
            HDRInstanceFinder.DataManager.BusTree.Dirty = true;
            //AudioBusVolumeHelper.UpdateCombinedVolume(HDRInstanceFinder.DataManager.BusTree);

        }
    }

    protected override bool CanDropObjects(AudioBus node, Object[] objects)
    {
        if (objects != null && objects.Length > 0 && objects[0] as AudioBus != null)
        {
            return !NodeWorker.IsChildOf(objects[0] as AudioBus, node);
        }
        return false;
    }

    protected override void OnDrop(AudioBus node, Object[] objects)
    {
        var target = objects[0] as AudioBus;

        if (UndoHelper.IsNewUndo)
        {
            EditorUtility.SetDirty(node.gameObject);
            UndoHelper.RegisterFullObjectHierarchyUndo(node);
        }
        else
        {
            UndoHelper.RecordObjects("Bus Move", node, target, target.Parent);
        }
        
        NodeWorker.ReasignNodeParent(target, node);            
    }

    protected override void OnContext(AudioBus audioBus)
    {
        var menu = new GenericMenu();

        menu.AddItem(new GUIContent(@"Create Child"), false, CreateChildBus, audioBus);

        menu.AddSeparator("");

        if (!audioBus.IsRoot)
            menu.AddItem(new GUIContent(@"Delete"), false, data => {
                treeDrawer.SelectPreviousNode();
                DeleteNode(audioBus);
            }, audioBus);
        else
            menu.AddDisabledItem(new GUIContent(@"Delete"));

        menu.ShowAsContext();
    }

    private void CreateChildBus(object userData)
    {
        AudioBus bus = userData as AudioBus;
        Undo.RegisterUndo(bus, "Bus Creation");
        AudioBusWorker.CreateBus(bus);
    }

    protected override bool OnNodeDraw(AudioBus node, bool isSelected)
    {
        return BusDrawer.Draw(node, isSelected);
    }

    private void DeleteNode(AudioBus bus)
    {
        Undo.RegisterUndo(bus.Parent, "Bus Deletion");
        AudioBusWorker.DeleteBus(bus, HDRInstanceFinder.DataManager.AudioTree);
    }

    public void FindBus(AudioBus audioBus)
    {
        searchingFor = audioBus.ID.ToString();
        lowercaseSearchingFor = searchingFor;
        treeDrawer.Filter(ShouldFilter);
    }
}
