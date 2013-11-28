using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections;
using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;

public static class TreeWalker
{
    public static void ForEach<T>(T node, Action<T> action)
        where T : Object, ITreeNode<T>
    {
        if (node == null)
            return;

        action(node);

        for (int i = 0; i < node.GetChildren.Count; i++)
        {
            ForEach<T>(node.GetChildren[i], action);
        }
    }

    public static List<U> FindAll<T, U>(T node, Func<T, U> toAdd) where T : Object, ITreeNode<T> where U : class
    {
        var found = new HashSet<U>();
        FindAll(node, toAdd, found);
        return found.ToList();
    }

    private static void FindAll<T, U>(T node, Func<T, U> toAdd, HashSet<U> found) where T: Object, ITreeNode<T> where U : class
    {
        if (node == null)
            return;
        U foundObj = toAdd(node);
        if(foundObj != null)
        {
            found.Add(foundObj);
        }
        for (int i = 0; i < node.GetChildren.Count; i++)
        {
            FindAll(node.GetChildren[i], toAdd, found);
        }
    }

    public static T FindById<T>(T node, int id) where T : Object, ITreeNode<T>
    {
        if (node == null)
            return null;
        if (node.ID == id)
            return node;
        for (int i = 0; i < node.GetChildren.Count; i++)
        {
            var result = FindById(node.GetChildren[i], id);
            if (result != null && result.ID == id)
                return result;
        }
        return null;
    }

    public static int Count<T>(T node, Func<T, bool> predicate) where T : Object, ITreeNode<T>
    {
        if (node == null)
            return 0;
        int result = 0;
        if (predicate(node))
            result += 1;
        for (int i = 0; i < node.GetChildren.Count; i++)
        {
            result += Count(node.GetChildren[i], predicate);
        }
        return result;
    }

    public static int FindIndexInParent<T>(T node) where T : Object, ITreeNode<T>
    {
        if (node.GetParent == null)
            return 0;
        for (int i = 0; i < node.GetParent.GetChildren.Count; ++i)
        {
            if (node.GetParent.GetChildren[i] == node)
            {
                return i;
            }
        }
        return 0;
    }


#if UNITY_EDITOR
    public static T FindFoldedParent<T>(T node) where T : Object, ITreeNode<T>
    {
        if (node.IsRoot)
        {
            return default(T); //Null
        }
        else
        {
            var parent = node.GetParent;
            if (!parent.IsFoldedOut)
                return parent;
            else
                return FindFoldedParent(parent);
        }
    }

    public static T FindPreviousUnfoldedNode<T>(T node) where T : Object, ITreeNode<T>
    {
        int index = FindIndexInParent(node);

        if (node.IsRoot)
            return node;
        else if (index == 0)
        {
            if (node.GetParent.IsRoot)
                return node.GetParent;
            return node.GetParent;
        }
        else
        {
            T previousNode = node.GetParent.GetChildren[index - 1];
            while (previousNode.IsFoldedOut)
            {
                if (previousNode.GetChildren.Count == 0)
                    return previousNode;
                else
                {
                    previousNode = previousNode.GetChildren[previousNode.GetChildren.Count - 1];
                }
            }
            return previousNode;
        }
    }

    public static T FindNextUnfoldedNode<T>(T node) where T : Object, ITreeNode<T>
    {
        if (node.IsFoldedOut && node.GetChildren.Count > 0)
            return node.GetChildren[0];

        T found = node;
        if (node.IsFoldedOut && node.GetChildren.Count == 0)
        {
            found = FindNextSibling(node);
            if (found.IsRoot) //Is root node
                return node;
            else 
                return found;
        }

        found = FindNextSibling(node);
        if (found.GetParent == null)
            return node;
        else 
            return found;
    }

#endif

    public static T FindNextNode<T>(T node) where T : Object, ITreeNode<T>
    {
        if (node.GetChildren.Count > 0)
            return node.GetChildren[0];

        T found = node;
        if (node.GetChildren.Count == 0)
        {
            found = FindNextSibling(node);
            if (found.IsRoot)
                return node;
            else 
                return found;
        }

        found = FindNextSibling(node);
        if (found.IsRoot)
            return node;
        else 
            return found;
    }

    public static T FindNextSibling<T>(T node) where T : Object, ITreeNode<T>
    {
        //Keep walking up as the current node may be n deep
        while (node != null && node.GetParent != null)
        {
            //Look through all the children
            for (int i = 0; i < node.GetParent.GetChildren.Count; ++i)
            {
                //We found the starting node
                if (node.GetParent.GetChildren[i] == node)
                {
                    //If the node is the last one, select the parent and try again by breaking to the while loop
                    if (i == node.GetParent.GetChildren.Count - 1)
                    {
                        node = node.GetParent;
                        break;
                    }
                    else //There is another sibling, select that one
                    {
                        return node.GetParent.GetChildren[i + 1];
                    }
                }
            }
        }
        return node;
    }

    public static T FindNextSibling<T>(T node, Func<T, bool> predicate) where T : Object, ITreeNode<T>
    {
        //Keep walking up as the current node may be n deep
        while (node != null && node.GetParent != null)
        {
            //Look through all the children
            for (int i = 0; i < node.GetParent.GetChildren.Count; ++i)
            {
                //We found the starting node
                if (node.GetParent.GetChildren[i] == node)
                {
                    //If the node is the last one, select the parent and try again by breaking to the while loop
                    if (i == node.GetParent.GetChildren.Count - 1)
                    {
                        node = node.GetParent;
                        break;
                    }
                    else //There is another sibling, select that one
                    {
                        return node.GetParent.GetChildren[i + 1];
                    }
                }
            }
        }
        return node;
    }




    public static T FindNextNode<T>(T node, Func<T, bool> predicate) where T : Object, ITreeNode<T>
    {
        if (node.IsFoldedOut && predicate(node) && node.GetChildren.Count > 0)
            return node.GetChildren[0];

        T found = node;
        if (node.IsFoldedOut && predicate(node) && node.GetChildren.Count == 0)
        {
            found = FindNextSibling(node, predicate);
            if (found.IsRoot) //Is root node
                return node;
            else
                return found;
        }

        found = FindNextSibling(node);
        if (found.GetParent == null)
            return node;
        else
            return found;
    }
}
