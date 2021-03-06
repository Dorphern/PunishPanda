﻿//using System.Collections.Generic;
//using InAudio;
//using UnityEngine;
//using System.Collections;

//public class InAudioSystem : MonoBehaviour
//{
//    /******************/
//    /*Public interface*/
//    /******************/

//    #region Post Event by reference
//    public static void PostEvent(GameObject controllingObject, AudioEvent postEvent)
//    {
//        if (instance != null && controllingObject != null && postEvent != null)
//            instance.OnPostEvent(controllingObject, postEvent, controllingObject);
//    }

//    public static void PostEventAttachedTo(GameObject controllingObject, AudioEvent postEvent, GameObject attachedToOther)
//    {
//        if (instance != null && controllingObject != null && postEvent != null)
//            instance.OnPostEvent(controllingObject, postEvent, attachedToOther);
//    }

//    public static void PostEventAtPosition(GameObject controllingObject, AudioEvent postEvent, Vector3 position)
//    {
//        if (instance != null && controllingObject != null && postEvent != null)
//            instance.OnPostEventAtPosition(controllingObject, postEvent, position);
//    }

//    public static void LoadBank(AudioBankLink bank)
//    {
//        if (bank != null)
//            BankLoader.Load(bank);
//    }

//    public static void UnloadBank(AudioBankLink bank)
//    {
//        if (bank != null)
//            BankLoader.Unload(bank);
//    }

//    public static void PostEvents(GameObject controllingObject, IList<AudioEvent> postEvent)
//    {
//        if (instance != null && controllingObject != null && postEvent != null)
//        {
//            int count = postEvent.Count;
//            for (int i = 0; i < count; i++)
//            {
//                AudioEvent audioEvent = postEvent[i];
//                if (audioEvent != null)
//                    instance.OnPostEvent(controllingObject, postEvent[i], controllingObject);
//            }

//        }
//    }

//    #endregion

//    #region Post Event By ID
//    public static void PostEvent(GameObject controllingObject, int eventID)
//    {
//        AudioEvent postEvent;
//        if (controllingObject != null && instance != null &&
//            instance.runtimeData.Events.TryGetValue(eventID, out postEvent))
//        {
//            if (postEvent != null)
//                instance.OnPostEvent(controllingObject, postEvent, controllingObject);
//        }
//    }

//    public static void PostEventAttachedTo(GameObject controllingObject, int eventID, GameObject attachedToOther)
//    {
//        AudioEvent postEvent;
//        if (controllingObject != null && instance != null &&
//            instance.runtimeData.Events.TryGetValue(eventID, out postEvent))
//        {
//            if (postEvent != null)
//                instance.OnPostEvent(controllingObject, postEvent, attachedToOther);
//        }
//    }

//    public static void PostEventAtPosition(GameObject controllingObject, int eventID, Vector3 position)
//    {
//        AudioEvent postEvent;
//        if (controllingObject != null && instance != null && instance.runtimeData.Events.TryGetValue(eventID, out postEvent))
//        {
//            if (postEvent != null)
//                instance.OnPostEvent(controllingObject, postEvent, position);
//        }
//    }

//    public static void LoadBank(int bankID)
//    {
//        var bank = TreeWalker.FindById(InAudioInstanceFinder.DataManager.BankLinkTree, bankID);
//        BankLoader.Load(bank);
//    }

//    public static void UnloadBank(int bankID)
//    {
//        var bank = TreeWalker.FindById(InAudioInstanceFinder.DataManager.BankLinkTree, bankID);
//        BankLoader.Unload(bank);
//    }
//    #endregion

//    #region Internal
//    #region Internal event handling

//    private void HandleEventAction(GameObject controllingObject, AudioEventAction eventData, GameObject attachedTo, Vector3 playAt = new Vector3())
//    {
//        AudioNode audioNode; //Because we can't create variables in the scope of the switch with the same name
//        EventBankAction bankData;

//        switch (eventData.EventActionType)
//        {
//            case EventActionTypes.Play:
//                audioNode = ((EventAudioAction)eventData).Node;
//                if (attachedTo != null)
//                    _runtimeEventWorker.PlayAttachedTo(controllingObject, audioNode, attachedTo);
//                else
//                    _runtimeEventWorker.PlayAtPosition(controllingObject, audioNode, playAt);
//                break;
//            case EventActionTypes.Stop:
//                audioNode = ((EventAudioAction)eventData).Node;
//                _runtimeEventWorker.StopByNode(controllingObject, audioNode);
//                break;
//            case EventActionTypes.StopAll:
//                _runtimeEventWorker.StopAll(controllingObject);
//                break;
//            case EventActionTypes.Break:
//                audioNode = ((EventAudioAction)eventData).Node;
//                _runtimeEventWorker.Break(controllingObject, audioNode);
//                break;
//            case EventActionTypes.SetBusVolume:
//                var busData = eventData as EventBusAction;
//                if (busData != null && busData.Bus != null)
//                {
//                    AudioBusVolumeHelper.SetTargetVolume(busData.Bus, busData.Volume, busData.VolumeMode, busData.Duration, busData.FadeCurve);
//                }
//                break;
//            case EventActionTypes.LoadBank:
//                bankData = eventData as EventBankAction;
//                if (bankData != null)
//                    BankLoader.Load(bankData.BankLink);
//                break;
//            case EventActionTypes.UnloadBank:
//                bankData = eventData as EventBankAction;
//                if (bankData != null)
//                    BankLoader.Unload(bankData.BankLink);
//                break;
//            case EventActionTypes.StopAllInBus:
//                busData = eventData as EventBusAction;
//                if (busData != null && busData.Bus != null)
//                {
//                    StopAllNodeInBus(busData.Bus);
//                }
//                break;
//        }
//    }
//    #region Post attached to
//    private void OnPostEvent(GameObject controllingObject, AudioEvent postEvent, GameObject attachedToOther)
//    {
//        bool areAnyDelayed = false;
//        if (postEvent.Delay > 0)
//        {
//            StartCoroutine(DelayedEvent(controllingObject, postEvent, attachedToOther));
//        }
//        else
//        {
//            areAnyDelayed = PostUndelayedActions(controllingObject, postEvent, attachedToOther);
//        }

//        if (areAnyDelayed)
//        {
//            for (int i = 0; i < postEvent.ActionList.Count; ++i)
//            {
//                var eventData = postEvent.ActionList[i];
//                if (eventData != null && eventData.Delay > 0)
//                    StartCoroutine(PostDelayedActions(controllingObject, eventData, attachedToOther));
//            }
//        }
//    }

//    private bool PostUndelayedActions(GameObject controllingObject, AudioEvent postEvent, GameObject attachedToOther)
//    {
//        bool areAnyDelayed = false;
//        for (int i = 0; i < postEvent.ActionList.Count; ++i)
//        {
//            var eventData = postEvent.ActionList[i];
//            if (eventData == null)
//                continue;
//            if (eventData.Delay > 0)
//            {
//                areAnyDelayed = true;
//            }
//            else
//                HandleEventAction(controllingObject, eventData, attachedToOther);
//        }
//        return areAnyDelayed;
//    }

//    private IEnumerator DelayedEvent(GameObject controllingObject, AudioEvent postEvent, GameObject attachedToOther)
//    {
//        yield return new WaitForSeconds(postEvent.Delay);
//        PostUndelayedActions(controllingObject, postEvent, attachedToOther);
//    }

//    private IEnumerator PostDelayedActions(GameObject controllingObject, AudioEventAction eventData, GameObject attachedToOther)
//    {
//        yield return new WaitForSeconds(eventData.Delay);
//        HandleEventAction(controllingObject, eventData, attachedToOther);
//    }
//    #endregion
//    #region Post at position
//    private void OnPostEventAtPosition(GameObject controllingObject, AudioEvent audioEvent, Vector3 position)
//    {
//        if (instance != null && controllingObject != null && audioEvent != null)
//            instance.OnPostEvent(controllingObject, audioEvent, position);
//    }

//    private void OnPostEvent(GameObject controllingObject, AudioEvent postEvent, Vector3 postAt)
//    {
//        bool areAnyDelayed = false;
//        if (postEvent.Delay > 0)
//        {
//            StartCoroutine(DelayedEvent(controllingObject, postEvent, postAt));
//        }
//        else
//        {
//            areAnyDelayed = PostUndelayedActions(controllingObject, postEvent, postAt);
//        }

//        if (areAnyDelayed)
//        {
//            for (int i = 0; i < postEvent.ActionList.Count; ++i)
//            {
//                var eventData = postEvent.ActionList[i];
//                if (eventData != null && eventData.Delay > 0)
//                    StartCoroutine(PostDelayedActions(controllingObject, eventData, postAt));
//            }
//        }
//    }

//    private bool PostUndelayedActions(GameObject controllingObject, AudioEvent postEvent, Vector3 postAt)
//    {
//        bool areAnyDelayed = false;
//        for (int i = 0; i < postEvent.ActionList.Count; ++i)
//        {
//            var eventData = postEvent.ActionList[i];
//            if (eventData == null)
//                continue;
//            if (eventData.Delay > 0)
//            {
//                areAnyDelayed = true;
//            }
//            else
//                HandleEventAction(controllingObject, eventData, null, postAt);
//        }
//        return areAnyDelayed;
//    }

//    private IEnumerator DelayedEvent(GameObject controllingObject, AudioEvent postEvent, Vector3 postAt)
//    {
//        yield return new WaitForSeconds(postEvent.Delay);
//        PostUndelayedActions(controllingObject, postEvent, postAt);
//    }

//    private IEnumerator PostDelayedActions(GameObject controllingObject, AudioEventAction eventData, Vector3 postAt)
//    {
//        yield return new WaitForSeconds(eventData.Delay);
//        HandleEventAction(controllingObject, eventData, null, postAt);
//    }
//    #endregion
//    #endregion

//    #region Internal data

//    void OnEnable()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            _runtimeEventWorker = GetComponentInChildren<RuntimeEventWorker>();
//            runtimeData = GetComponentInChildren<RuntimeAudioData>();
//            BankLoader.LoadAutoLoadedBanks();
//            runtimeData.UpdateEvents(InAudioInstanceFinder.DataManager.EventTree);
//            AudioBusVolumeHelper.UpdateCombinedVolume(InAudioInstanceFinder.DataManager.BusTree);
//            DontDestroyOnLoad(transform.parent.gameObject);
//        }
//        else
//        {
//            Object.Destroy(transform.parent);
//        }
//    }

//    private RuntimeEventWorker _runtimeEventWorker;

//    private RuntimeAudioData runtimeData;

//    private static InAudioSystem instance;


//    //TODO Move this to another class
//    private static void StopAllNodeInBus(AudioBus bus)
//    {
//        var players = bus.GetRuntimePlayers();
//        for (int i = 0; i < players.Count; i++)
//        {
//            players[i].Stop();
//        }
//        for (int i = 0; i < bus.Children.Count; i++)
//        {
//            StopAllNodeInBus(bus.Children[i]);
//        }
//    }

//    #endregion


//    #region Unity functions
//    void FixedUpdate()
//    {
//        AudioBusVolumeHelper.UpdateBusVolumes(InAudioInstanceFinder.DataManager.BusTree);
//    }

//    void Start()
//    {
//        if (InAudioInstanceFinder.DataManager != null && InAudioInstanceFinder.DataManager.BusTree != null)
//            InAudioInstanceFinder.DataManager.BusTree.Dirty = true;
//    }
//    #endregion


//    #endregion
//}