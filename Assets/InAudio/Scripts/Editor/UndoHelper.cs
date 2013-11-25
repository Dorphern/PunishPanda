using UnityEditor;
using UnityEngine;
using System.Collections;
using InAudio.ExtensionMethods;
using Object = UnityEngine.Object;

namespace InAudio
{
    public delegate void Action();

    public static class UndoHelper
    {
        public static bool IsNewUndo
        {
            get
            {
#if UNITY_4_1 || UNITY_4_2
                return false;
#else
            return true;
#endif
            }
        }

        public static Object[] Array(params Object[] obj)
        {
            return obj;
        }

        public static void DoInGroup(Action action)
        {
            IncrementGroup();
            action();
            EndGroup();
        }

        public static bool DisplayUndoWarning()
        {
            if (!IsNewUndo)
            {
                return EditorUtility.DisplayDialog("Cannot Undo", "This action cannot be undone as you are running Unity" + Application.unityVersion + ".\nUpgrade to 4.3 or later to enable undo", "Do Action", "Cancel");
            }
            return true;
        }

        public static void IncrementGroup()
        {
#if !UNITY_4_1 && !UNITY_4_2
            Undo.IncrementCurrentGroup();
#endif
        }

        public static void EndGroup()
        {
#if !UNITY_4_1 && !UNITY_4_2
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            Undo.IncrementCurrentGroup();
#endif
        }

        public static void RecordObject(Object obj, string undoDescription)
        {
#if UNITY_4_1 || UNITY_4_2
            Undo.RegisterUndo(obj, undoDescription);
#else
            Undo.RecordObject(obj, undoDescription);
#endif
        }

        public static void RecordObjects(Object[] obj, string undoDescription)
        {
            Object[] nonNulls = obj.TakeNonNulls();
#if UNITY_4_1 || UNITY_4_2
            Undo.RegisterUndo(nonNulls, undoDescription);
#else
            Undo.RecordObjects(nonNulls, undoDescription);
#endif
        }

        public static void RecordObjects(string undoDescription, params Object[] obj)
        {
            RecordObjects(undoDescription, obj);
        }

        public static void RecordDestroy(Object obj)
        {
#if !UNITY_4_1 && !UNITY_4_2
        Undo.DestroyObjectImmediate(obj);
#endif
        }

        public static void RecordDestroy(Object[] obj)
        {
#if !UNITY_4_1 && !UNITY_4_2
        for (int i = 0; i < obj.Length; i++)
	    {
		    Undo.DestroyObjectImmediate(obj[i]);
	    }
#endif
        }

        public static Object[] NodeUndo(AudioNode node)
        {
            return new Object[]
        {
            node,
            node.NodeData,
            node.GetBank().LazyBankFetch
        };
        }

        public static T AddComponent<T>(GameObject go) where T : Component
        {
            return AddComponentUndo(go, typeof(T)) as T;
        }

        public static Object AddComponentUndo(this GameObject go, System.Type type)
        {
#if UNITY_4_1 || UNITY_4_2
            return go.AddComponent(type);
#else 
            return Undo.AddComponent(go, type);
#endif
        }

        public static T AddComponentUndo<T>(this GameObject go) where T : Component
        {
            return AddComponent<T>(go);
        }

        public static void CompleteObjectUndo(Object obj, string description)
        {
#if !UNITY_4_1 && !UNITY_4_2
            Undo.RegisterCompleteObjectUndo(obj, description);
#endif
        }

        public static void RegisterFullObjectHierarchyUndo(Object obj)
        {
#if !UNITY_4_1 && !UNITY_4_2
            Undo.RegisterFullObjectHierarchyUndo(obj);
#endif
        }

        public static void DestroyObjectImmediate(Object obj)
        {
#if !UNITY_4_1 && !UNITY_4_2
            Undo.DestroyObjectImmediate(obj);
#endif
        }

        public static void DragNDropUndo(Object obj, string description)
        {
#if UNITY_4_1 || UNITY_4_2
            Undo.RegisterUndo(obj, description);
#else 
            if(obj != null)
                Undo.RegisterFullObjectHierarchyUndo(obj);
#endif

        }

        
    }
}