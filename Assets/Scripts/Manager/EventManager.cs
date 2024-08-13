using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {
    public bool limitQueueProcessing;
    public float queueProcessTime;
    static EventManager sInstance;
    readonly Queue mEventQueue = new Queue();

    public delegate void EventDelegate<in T> (T e) where T : GameEvent;
    delegate void EventDelegate (GameEvent e);

    readonly Dictionary<Type, EventDelegate> delegates = new Dictionary<Type, EventDelegate>();
    readonly Dictionary<Delegate, EventDelegate> delegateDictionary = new Dictionary<Delegate, EventDelegate>();
    readonly Dictionary<Delegate, Delegate> onceLookups = new Dictionary<Delegate, Delegate>();
    
    public static EventManager Instance {
        get {
            if (sInstance == null) {
                sInstance = FindObjectOfType (typeof(EventManager)) as EventManager;
            }
            return sInstance;
        }
    }

    EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : GameEvent {
        if (delegateDictionary.ContainsKey(del))
            return null;
        
        EventDelegate internalDelegate = e => del((T)e);
        delegateDictionary[del] = internalDelegate;

        if (delegates.TryGetValue(typeof(T), out EventDelegate tempDel)) {
            delegates[typeof(T)] = tempDel + internalDelegate; 
        } else delegates[typeof(T)] = internalDelegate;

        return internalDelegate;
    }

    public void AddListener<T> (EventDelegate<T> del) where T : GameEvent {
        AddDelegate(del);
    }

    public void AddListenerOnce<T> (EventDelegate<T> del) where T : GameEvent {
        EventDelegate result = AddDelegate(del);

        if(result != null) onceLookups[result] = del;
    }

    public void RemoveListener<T> (EventDelegate<T> del) where T : GameEvent
    {
        if (!delegateDictionary.TryGetValue(del, out EventDelegate internalDelegate)) return;
        if (delegates.TryGetValue(typeof(T), out EventDelegate tempDel)){
            tempDel -= internalDelegate;
            if (tempDel == null) delegates.Remove(typeof(T));
            else delegates[typeof(T)] = tempDel;
        }

        delegateDictionary.Remove(del);
    }

    void RemoveAll(){
        delegates.Clear();
        delegateDictionary.Clear();
        onceLookups.Clear();
    }

    /*public bool HasListener<T> (EventDelegate<T> del) where T : GameEvent {
        return delegateDictionary.ContainsKey(del);
    }*/

    void TriggerEvent (GameEvent e) {
        if (delegates.TryGetValue(e.GetType(), out EventDelegate del)) {
            del.Invoke(e);

            // remove listeners which should only be called once
            foreach(Delegate @delegate in delegates[e.GetType()].GetInvocationList())
            {
                EventDelegate k = (EventDelegate)@delegate;
                if (!onceLookups.ContainsKey(k)) continue;
                delegates[e.GetType()] -= k;

                if(delegates[e.GetType()] == null) delegates.Remove(e.GetType());

                delegateDictionary.Remove(onceLookups[k]);
                onceLookups.Remove(k);
            }
        } else {
            Debug.LogWarning("Event: " + e.GetType() + " has no listeners");
        }
    }

    //Inserts the event into the current queue.
    public bool QueueEvent(GameEvent evt) {
        if (!delegates.ContainsKey(evt.GetType())) {
            Debug.LogWarning("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetType());
            return false;
        }

        mEventQueue.Enqueue(evt);
        return true;
    }

    //Every update cycle the queue is processed, if the queue processing is limited,
    //a maximum processing time per update can be set after which the events will have
    //to be processed next update loop.
    void Update() {
        float timer = 0.0f;
        while (mEventQueue.Count > 0) {
            if (limitQueueProcessing) {
                if (timer > queueProcessTime)
                    return;
            }

            GameEvent evt = mEventQueue.Dequeue() as GameEvent;
            TriggerEvent(evt);

            if (limitQueueProcessing)
                timer += Time.deltaTime;
        }
    }

    public void OnApplicationQuit(){
        RemoveAll();
        mEventQueue.Clear();
        sInstance = null;
    }
}