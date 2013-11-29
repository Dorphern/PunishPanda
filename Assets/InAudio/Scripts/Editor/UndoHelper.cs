using System;
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
#if !UNITY_4_1 && !UNITY_4_2
            Undo.IncrementCurrentGroup();
#endif

            action();

#if !UNITY_4_1 && !UNITY_4_2
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            Undo.IncrementCurrentGroup();
#endif
        }

        public static bool DoInGroupWithWarning(Action action)
        {
#if !UNITY_4_1 && !UNITY_4_2
            Undo.IncrementCurrentGroup();
#endif 

            bool delete = DeleteDialogue(action);

#if !UNITY_4_1 && !UNITY_4_2
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            Undo.IncrementCurrentGroup();
#endif
            return delete;
        }

        public static void RecordObject(Object obj, string undoDescription)
        {
            EditorUtility.SetDirty(obj);
#if UNITY_4_1 || UNITY_4_2
            Undo.RegisterUndo(obj, undoDescription);
#else
            RecordObjectFull(obj, undoDescription);
#endif
        }

        public static void RecordObject(Object[] obj, string undoDescription)
        {
            Object[] nonNulls = obj.TakeNonNulls();
            for (int i = 0; i < nonNulls.Length; i++)
            {
                EditorUtility.SetDirty(nonNulls[i]);
            }
#if UNITY_4_1 || UNITY_4_2
            Undo.RegisterUndo(nonNulls, undoDescription);
#else 
            RecordObjectFull(nonNulls, undoDescription);
#endif
        }

        public static void RecordObjectFull(Object obj, string undoDescription)
        {
            EditorUtility.SetDirty(obj);
            
#if UNITY_4_1 || UNITY_4_2
            Undo.RegisterUndo(obj, undoDescription);
#else
            Undo.RegisterCompleteObjectUndo(obj, undoDescription);
#endif
        }


        public static void RecordObjectFull(Object[] obj, string undoDescription)
        {
            Object[] nonNulls = obj.TakeNonNulls();
            for (int i = 0; i < nonNulls.Length; i++)
            {
                EditorUtility.SetDirty(nonNulls[i]);
            }
#if UNITY_4_1 || UNITY_4_2
            Undo.RegisterUndo(nonNulls, undoDescription);
#else
            Undo.RegisterCompleteObjectUndo(nonNulls, undoDescription);
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

        public static Object AddComponentUndo(this GameObject go, Type type)
        {
            EditorUtility.SetDirty(go);
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
            EditorUtility.SetDirty(obj);
#if !UNITY_4_1 && !UNITY_4_2
            Undo.RegisterCompleteObjectUndo(obj, description);
#endif
        }

        public static void RegisterUndo(Object obj, string description)
        {
            EditorUtility.SetDirty(obj);
#if !UNITY_4_1 && !UNITY_4_2
            Undo.RegisterCompleteObjectUndo(obj, description);
#else
            Undo.RegisterUndo(obj, description);
#endif

        }

        public static void RegisterFullObjectHierarchyUndo(Object obj)
        {
            EditorUtility.SetDirty(obj);
#if !UNITY_4_1 && !UNITY_4_2
            Undo.RegisterFullObjectHierarchyUndo(obj);
#endif
        }

        public static void Destroy(Object obj)
        {
            SceneView.RepaintAll();
            HandleUtility.Repaint();
            EditorUtility.SetDirty(obj);
#if !UNITY_4_1 && !UNITY_4_2
            if(obj != null)
                Undo.DestroyObjectImmediate(obj);
#else
            if(obj != null)
                Object.DestroyImmediate(obj, true);
#endif
        }

        public static void DestroyOnlyInNew(Object obj)
        {
#if !UNITY_4_1 && !UNITY_4_2
            if (obj != null)
                Undo.DestroyObjectImmediate(obj);
#endif
        }


        public static void DragNDropUndo(Object obj, string description)
        {
            EditorUtility.SetDirty(obj);
#if UNITY_4_1 || UNITY_4_2
            Undo.RegisterUndo(obj, description);
#else 
            if(obj != null)
                Undo.RegisterFullObjectHierarchyUndo(obj);
#endif

        }

        public static void GUIUndo<T>(Object obj, string description, Func<T> displayFunction, Action<T> assignAction) 
        {
            EditorGUI.BeginChangeCheck();
            T newValue = displayFunction();
            if (EditorGUI.EndChangeCheck())
            {
                RecordObjectFull(obj, description);
                assignAction(newValue);
                EditorUtility.SetDirty(obj);
            }
        }

        public static void GUIUndo<T>(Object obj, string description, ref T value, Func<T> displayFunction)
        {
            EditorGUI.BeginChangeCheck();
            T newValue = displayFunction();
            if (EditorGUI.EndChangeCheck())
            {
                RecordObjectFull(obj, description);
                value = newValue;
                EditorUtility.SetDirty(obj);
            }
        }

        public delegate void RefOut<T>(out T v1, out T v2);
        public delegate void RefAssign<in T>(T v1, T v2);
        public static void GUIUndo<T>(Object obj, string description, RefOut<T> displayFunction, RefAssign<T> assignAction)
        {
            EditorGUI.BeginChangeCheck();
            T v1;
            T v2;
            displayFunction(out v1, out v2);
            if (EditorGUI.EndChangeCheck())
            {
                RecordObjectFull(obj, description);
                assignAction(v1, v2);
                EditorUtility.SetDirty(obj);
            }
        }

        public static void GUIUndo<T>(Object obj, string description, ref T value1, ref T value2, RefOut<T> displayFunction)
        {
            EditorGUI.BeginChangeCheck();
            T v1;
            T v2;
            displayFunction(out v1, out v2);
            if (EditorGUI.EndChangeCheck())
            {
                RecordObjectFull(obj, description);
                value1 = v1;
                value2 = v2;
                EditorUtility.SetDirty(obj);
            }
        }

        public static bool DeleteDialogue(Action action)
        {
            #if UNITY_4_1 || UNITY_4_2
                bool delete = EditorUtility.DisplayDialog("Delete Item?",
                    "Deleting this cannot be undo as you are running a version previous to Unity 4.3. Delete anyway?",
                    "Delete", "Do Nothing"); 
                if (delete)
                {
                    action();
                }
                return delete;
             #else
                action();
                return true;
            #endif

        }
    }
}