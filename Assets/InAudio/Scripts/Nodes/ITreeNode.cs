using System.Collections.Generic;

public interface ITreeNode<T> where T : UnityEngine.Object, ITreeNode<T>
{
    T GetParent { get; set; }

    List<T> GetChildren { get; }

    bool IsRoot { get; }

    string GetName { get; }

    //T Node { get; }

    int ID { get; set; }

#if UNITY_EDITOR
    bool IsFoldedOut { get; set; }

    bool IsFiltered { get; set; }
#endif
}
