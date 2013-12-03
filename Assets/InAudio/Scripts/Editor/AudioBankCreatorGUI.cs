using System;
using InAudio;
using InAudio.InAudioEditorGUI;
using InAudio.TreeDrawer;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class AudioBankCreatorGUI : BaseCreatorGUI<AudioBankLink>
{
    public AudioBankCreatorGUI(AuxWindow window) : base(window)
    {
        this.window = window;
    }

    private int leftWidth;
    private int height;

    public bool OnGUI(int leftWidth, int height)
    {
        BaseOnGUI();

        var dataManager = InAudioInstanceFinder.DataManager;
        if (dataManager != null)
        {
            var root = dataManager.BankLinkTree;
            int id = InAudioInstanceFinder.InAudioGuiUserPrefs.SelectedBankLinkID;
            var selectedNode = UpdateSelectedNode(root, id);
            InAudioInstanceFinder.InAudioGuiUserPrefs.SelectedBankLinkID = selectedNode != null ? selectedNode.ID : 0;
        }

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

        isDirty |= treeDrawer.DrawTree(InAudioInstanceFinder.DataManager.BankLinkTree, treeArea);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();
    }

    private void DrawRightSide(Rect area)
    {
        EditorGUILayout.BeginVertical();

        if (SelectedNode != null)
        {
            AudioBankLinkDrawer.Draw(SelectedNode);
        }

        EditorGUILayout.EndVertical();
    }

    protected override bool CanDropObjects(AudioBankLink node, Object[] objects)
    {
        if (node == null || objects == null)
            return false;

        if (objects.Length > 0 && objects[0] as AudioBankLink != null && node.Type != AudioBankTypes.Link)
        {
            return !NodeWorker.IsChildOf(objects[0] as AudioBankLink, node);
        }
        return false;
    }

    protected override bool OnNodeDraw(AudioBankLink node, bool isSelected)
    {
        return GenericTreeNodeDrawer.Draw(node, isSelected);
    }

    protected override void OnDrop(AudioBankLink node, Object[] objects)
    {
        UndoHelper.DragNDropUndo(node.Parent, "Bank Drag N Drop");
        AudioBankLink target = objects[0] as AudioBankLink;
        NodeWorker.ReasignNodeParent(target, node);
    }

    protected override void OnContext(AudioBankLink node)
    {
        if (node == null)
            return;
        var menu = new GenericMenu();
        
        if(node.Type == AudioBankTypes.Folder)
        {
            menu.AddItem(new GUIContent(@"Create Child/Folder"), false, data => CreateBank(node, AudioBankTypes.Folder), node);
            menu.AddItem(new GUIContent(@"Create Child/Bank"), false, data => CreateBank(node, AudioBankTypes.Link), node);
        }
        else if (node.Type == AudioBankTypes.Link) 
        {
            menu.AddDisabledItem(new GUIContent(@"Create Child/Folder"));
            menu.AddDisabledItem(new GUIContent(@"Create Child/Bank"));
        }

        menu.AddSeparator("");

        /*if (!toDelete.IsRoot)
        {
            menu.AddItem(new GUIContent(@"Delete"), false, data => DeleteNode(InAudioInstanceFinder.DataManager.BankLinkTree, data as AudioBankLink), toDelete);
        }
        else*/
        if (node.IsRoot)
            menu.AddDisabledItem(new GUIContent(@"Cannot Delete Root"));
        else
        {
            menu.AddItem(new GUIContent(@"Delete If Empty"), false, data => DeleteNode(InAudioInstanceFinder.DataManager.BankLinkTree, data as AudioBankLink), node);
        }
        menu.ShowAsContext();
    }

    private void DeleteNode(AudioBankLink root, AudioBankLink toDelete)
    {
        if (toDelete.GetChildren.Count > 0)
        {
            EditorUtility.DisplayDialog("Cannot delete bank", "Cannot delete folder with bank children", "ok");
            return;
        }


        Func<AudioNode, bool> usedBankRoot = node =>
        {
            if (node.Type == AudioNodeType.Folder)
            {
                if (node.IsRoot && node.BankLink == toDelete)
                {
                    return true;
                } 
                else if (node.Type == AudioNodeType.Folder && node.BankLink == toDelete )
                {
                    return true;
                }
            }

            return false;
        };
        
        if(TreeWalker.Any(InAudioInstanceFinder.DataManager.AudioTree, usedBankRoot))
        {
            EditorUtility.DisplayDialog("Cannot delete bank", "Cannot delete bank that is in use", "ok");
            return;
        }

        int nonFolderCount = TreeWalker.Count(root, link => link.Type == AudioBankTypes.Link);
        if (nonFolderCount == 1 && toDelete.Type == AudioBankTypes.Link)
        {
            EditorUtility.DisplayDialog("Cannot delete the bank", "Cannot delete the last bank", "ok");
            return;
        }

        if (toDelete.Type == AudioBankTypes.Link && !EditorUtility.DisplayDialog("Delete bank?", "This cannot be undone.", "Delete", "Do nothing"))
            return;

        if (toDelete.Type == AudioBankTypes.Link)
            AudioBankWorker.DeleteBank(toDelete);
        else if (toDelete.Type == AudioBankTypes.Folder)
            AudioBankWorker.DeleteFolder(toDelete);
    }

    private void CreateBank(AudioBankLink parent, AudioBankTypes type)
    {
        //TODO make real undo
        UndoHelper.RecordObjectFull(parent, "Bank " + (type == AudioBankTypes.Folder ? "Folder " : "") + "Creation");
        if (type == AudioBankTypes.Folder)
            AudioBankWorker.CreateFolder(parent.gameObject, parent, GUIDCreator.Create());
        else
            AudioBankWorker.CreateBank(parent.gameObject, parent, GUIDCreator.Create());
    }

    public override AudioBankLink Root()
    {
        return InAudioInstanceFinder.DataManager.BankLinkTree;
    }
}