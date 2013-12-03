using InAudio;
using InAudio.ExtensionMethods;
using InAudio.TreeDrawer;
using UnityEditor;
using UnityEngine;

public abstract class BaseCreatorGUI<T> where T : Object, ITreeNode<T>
{
    protected InAudioBaseWindow window;

    public TreeDrawer<T> treeDrawer = new TreeDrawer<T>();

    public T SelectedNode
    {
        get;
        set;
    }

    protected GUISkin inspectorSkin;

    public delegate void DrawSelectedNodeDelegate(T toDraw);

    public DrawSelectedNodeDelegate DrawSelectedNode;

    protected bool isDirty;

    protected string lowercaseSearchingFor;
    protected string searchingFor;

    protected BaseCreatorGUI(InAudioBaseWindow window)
    {
        this.window = window;
    }

    public void BaseOnGUI()
    {
        
        isDirty = false;
        if (!UndoHelper.IsNewUndo)
        {
            if (inspectorSkin == null)
            {
                inspectorSkin = CreaterGUIHelper.GetEditorSkin();
            }
        }

        if (Event.current.IsKeyDown(KeyCode.W) && Event.current.control)
        {
            window.Close();
        }
    }

    public abstract T Root();

    public virtual void OnEnable()
    {
        treeDrawer.Filter(n => false);
        treeDrawer.OnContext = OnContext;
        treeDrawer.OnDrop = OnDrop;
        treeDrawer.CanDropObjects = CanDropObjects;
        treeDrawer.OnNodeDraw = OnNodeDraw;
    }

    protected virtual void DrawSearchBar()
    {
      
        EditorGUILayout.BeginHorizontal();
        GUI.SetNextControlName("SearchBar");
        EditorGUILayout.LabelField("Search", GUILayout.Width(45));
        var content = EditorGUILayout.TextField(searchingFor);

        if (content != searchingFor)
        {
            searchingFor = content;
            lowercaseSearchingFor = searchingFor.ToLower().Trim();
            treeDrawer.Filter(SearchFilter);
            
        }

        if (GUILayout.Button("x", GUILayout.Width(25)) && Event.current.type != EventType.Repaint)
        {
            treeDrawer.Filter(SearchFilter);
            searchingFor = "";
            lowercaseSearchingFor = "";
            treeDrawer.FocusOnSelectedNode();

            GUI.FocusControl(null);
        }
        /**/
        GUILayout.EndHorizontal();
        
    }

    public virtual void OnUpdate()
    {
    }

    protected virtual bool OnNodeDraw(T node, bool isSelected)
    {
        return GenericTreeNodeDrawer.Draw(node, isSelected);
    }

    protected abstract bool CanDropObjects(T node, Object[] objects);

    protected abstract void OnDrop(T node, Object[] objects);

    protected abstract void OnContext(T node);

    public virtual void Find(T node)
    {
        searchingFor = node.ID.ToString();
        lowercaseSearchingFor = searchingFor.ToLower().Trim();
        treeDrawer.Filter(SearchFilter);
        TreeWalker.ForEachParent(node, n => n.IsFoldedOut = true);
        SelectedNode = node;
        treeDrawer.SelectedNode = node;
    }

    protected virtual bool SearchFilter(T node)
    {
        if (string.IsNullOrEmpty(lowercaseSearchingFor))
            return false;
        else
        {
            //Check name
            bool nameFiltered = !node.GetName.ToLower().Contains(lowercaseSearchingFor);
            //If name doesn't match, check if ID matches
            if (nameFiltered)
            {
                nameFiltered = !node.ID.ToString().Contains(lowercaseSearchingFor);
            }
            return nameFiltered;
        }
    }

    protected T UpdateSelectedNode(T root, int id)
    {
        if (treeDrawer.SelectedNode == null)
        {
            var found =  TreeWalker.FindById(root, id);
            if (found != null)
            {
                treeDrawer.SelectedNode = found;
            }
            else
            {
                treeDrawer.SelectedNode = root;
            }
        }
            
        SelectedNode = treeDrawer.SelectedNode;
        
        return treeDrawer.SelectedNode;
    }
}
