using System;
using System.Collections.Generic;
using System.Linq;
using InAudio.ExtensionMethods;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace InAudio
{
public static class AudioEventWorker  {
    private static AudioEvent CreateRoot(GameObject go, int guid)
    {
        var node = go.AddComponentUndo<AudioEvent>();
        node.Type = EventNodeType.Root;
        node.GUID = guid;
        node.FoldedOut = true;
        node.Name = "Root";
        return node;
    }

    private static AudioEvent CreateFolder(GameObject go, int guid, AudioEvent parent)
    {
        var node = go.AddComponentUndo<AudioEvent>();
        node.Type = EventNodeType.Folder;
        node.GUID = guid;
        node.Name = parent.Name + " Child";
        node.AssignParent(parent);
        return node;
    }

    public static void DeleteNode(AudioEvent node)
    {
        UndoHelper.DoInGroupWithWarning(() => DeleteNodeRec(node));
    }

    private static void DeleteNodeRec(AudioEvent node)
    {
        UndoHelper.RegisterUndo(node.Parent, "Event Deletion");
        for (int i = 0; i < node.Children.Count; ++i)
        {
            DeleteNodeRec(node.Children[i]);
        }
        node.Parent.Children.Remove(node); 
        UndoHelper.Destroy(node);
    }

   
    private static AudioEvent CreateEvent(GameObject go, AudioEvent parent, int guid, EventNodeType type)
    {
        var node = go.AddComponentUndo<AudioEvent>();
        node.Type = type;
        node.GUID = guid;
        node.Name = parent.Name + " Child";
        node.AssignParent(parent);
        return node;
    }

    public static AudioEvent CreateTree(GameObject go, int levelSize)
    {
        var tree = CreateRoot(go, GUIDCreator.Create());

        for (int i = 0; i < levelSize; ++i)
        {
            CreateFolder(go, GUIDCreator.Create(), tree);
        }

        return tree;
    }

    public static AudioEvent CreateNode(AudioEvent parent, EventNodeType type)
    {
        var child = CreateEvent(parent.gameObject, parent, GUIDCreator.Create(), type);
        child.FoldedOut = true;
        
        return child;
    }

    public static void ReplaceActionDestructiveAt(AudioEvent audioEvent, EventActionTypes enumType, int toRemoveAndInsertAt)
    {
        //A reel mess this function.
        //It adds a new component of the specied type, replaces the current at the toRemoveAndInsertAt index, and then deletes the old one
        float delay = audioEvent.ActionList[toRemoveAndInsertAt].Delay;
        var newActionType = ActionEnumToType(enumType);
        UndoHelper.Destroy(audioEvent.ActionList[toRemoveAndInsertAt]);
        UndoHelper.RecordObject(audioEvent, "Event Action Creation");
        audioEvent.ActionList.RemoveAt(toRemoveAndInsertAt);
        var added = AddEventAction(audioEvent, newActionType, enumType);
        added.Delay = delay;
        audioEvent.ActionList.Insert(toRemoveAndInsertAt, added);
        audioEvent.ActionList.RemoveLast();
    }

    public static T AddEventAction<T>(AudioEvent audioevent, EventActionTypes enumType) where T : AudioEventAction
    {
        var eventAction = audioevent.gameObject.AddComponentUndo<T>();
        audioevent.ActionList.Add(eventAction);
        eventAction.EventActionType = enumType;
        return eventAction;
    }

    public static AudioEventAction AddEventAction(AudioEvent audioevent, Type eventActionType, EventActionTypes enumType) 
    {
        UndoHelper.RecordObject(audioevent, "Event Action Creation");
        var eventAction = audioevent.gameObject.AddComponentUndo(eventActionType) as AudioEventAction;
        audioevent.ActionList.Add(eventAction);
        eventAction.EventActionType = enumType;

        return eventAction;
    }

    public static AudioEvent DeleteActionAtIndex(AudioEvent audioevent, int index)
    {
        UndoHelper.DoInGroup(() =>
        {
            UndoHelper.DestroyOnlyInNew(audioevent.ActionList[index]);
            UndoHelper.RecordObject(audioevent, "Event Action Creation");
        });
        audioevent.ActionList.RemoveAt(index);

        return audioevent;
    }

    public static AudioEvent Duplicate(AudioEvent audioEvent)
    {
        return NodeWorker.DuplicateHierarchy(audioEvent, (@oldNode, newNode) =>
        {
            newNode.ActionList.Clear();
            for (int i = 0; i < oldNode.ActionList.Count; i++)
            {
                newNode.ActionList.Add(NodeWorker.CopyComponent(oldNode.ActionList[i]));
            }
        });
    }


    public static Type ActionEnumToType(EventActionTypes actionType)
    {
        switch(actionType)
        {
            case EventActionTypes.Play:
                return typeof( EventAudioAction);
            case EventActionTypes.Stop:
                return typeof( EventAudioAction);
            case EventActionTypes.StopAll:
                return typeof( EventAudioAction);
            case EventActionTypes.LoadBank:
                return typeof( EventBankAction);
            case EventActionTypes.UnloadBank:
                return typeof(EventBankAction);
            case EventActionTypes.SetBusVolume:
                return typeof( EventBusAction);
            case EventActionTypes.Break:
                return typeof( EventAudioAction);
            case EventActionTypes.StopAllInBus:
                return typeof( EventBusAction);
        }
        return null;
    }

    public static bool CanDropObjects(AudioEvent audioEvent, Object[] objects)
    {
        if (objects.Length == 0 || audioEvent == null)
            return false;

        if (audioEvent.Type == EventNodeType.Event)
        {
            var audioNodes = GetConvertedList<AudioNode>(objects.ToList());
            bool audioNodeDrop = audioNodes.TrueForAll(node => node != null && node.IsPlayable);

            var audioBankLinks = GetConvertedList<AudioBankLink>(objects.ToList());
            bool bankLinkDrop = audioBankLinks.TrueForAll(node => node != null && node.Type == AudioBankTypes.Link);

            var busNodes = GetConvertedList<AudioBus>(objects.ToList());
            bool audioBusDrop = busNodes.TrueForAll(node => node != null);

            return audioNodeDrop | bankLinkDrop | audioBusDrop;
        }
        else if (audioEvent.Type == EventNodeType.Folder || audioEvent.Type == EventNodeType.Root)
        {
            var draggingEvent = objects[0] as AudioEvent;
            if (draggingEvent == null)
                return false;

            if (draggingEvent.Type == EventNodeType.Event)
                return true;
            if ((draggingEvent.Type == EventNodeType.Folder && !NodeWorker.IsChildOf(draggingEvent, audioEvent)) || draggingEvent.Type == EventNodeType.EventGroup)
                return true;
        }
        else if (audioEvent.Type == EventNodeType.EventGroup)
        {
            var draggingEvent = objects[0] as AudioEvent;
            if (draggingEvent == null)
                return false;
            if (draggingEvent.Type == EventNodeType.Event)
                return true;
        }

        return false;
    }

    private static List<T> GetConvertedList<T>(List<Object> toConvert) where T : class
    {
        return toConvert.ConvertAll(obj => obj as T);
    }

    public static void OnDrop(AudioEvent audioevent, Object[] objects)
    {
        if (objects.Length == 1)
        {
            if (objects[0] as AudioEvent)
            {
                var movingEvent = objects[0] as AudioEvent;
                
                if(UndoHelper.IsNewUndo)
                    UndoHelper.RegisterFullObjectHierarchyUndo(audioevent);
                else
                    UndoHelper.RecordObject(new Object[] { audioevent, movingEvent, movingEvent.Parent }, "Event Move");
                NodeWorker.ReasignNodeParent((AudioEvent)objects[0], audioevent);
                audioevent.IsFoldedOut = true;
            }

            var audioNode = objects[0] as AudioNode;
            if (audioNode != null && audioNode.IsPlayable)
            {
                UndoHelper.RecordObject(audioevent, "Adding of Audio Action");
                var action = AddEventAction<EventAudioAction>(audioevent,
                    EventActionTypes.Play);
                action.Node = audioNode;
            }

            var audioBank = objects[0] as AudioBankLink;
            if (audioBank != null)
            {
                UndoHelper.RecordObject(audioevent, "Adding of Bank Load Action");
                var action = AddEventAction<EventBankAction>(audioevent,
                    EventActionTypes.LoadBank);
                action.BankLink = audioBank;
            }

            var audioBus = objects[0] as AudioBus;
            if (audioBus != null)
            {
                UndoHelper.RecordObject(audioevent, "Adding of Bus Volume");
                var action = AddEventAction<EventBusAction>(audioevent,
                    EventActionTypes.SetBusVolume);
                action.Bus = audioBus;
            }
            Event.current.Use();
        }
    }

}
}
