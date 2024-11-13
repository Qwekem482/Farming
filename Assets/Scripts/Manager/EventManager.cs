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

    readonly Dictionary<Delegate, Delegate> lookups = new Dictionary<Delegate, Delegate>();
    readonly Dictionary<Type, EventDelegate> del = new Dictionary<Type, EventDelegate>();
    readonly Dictionary<Delegate, EventDelegate> delDict = new Dictionary<Delegate, EventDelegate>();
    
    public static EventManager Instance {
        get {
            if (sInstance == null) {
                sInstance = FindObjectOfType (typeof(EventManager)) as EventManager;
            }
            return sInstance;
        }
    }

    EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : GameEvent {
        if (delDict.ContainsKey(del))
            return null;
        
        EventDelegate inDelegate = e => del((T)e);
        delDict[del] = inDelegate;

        if (this.del.TryGetValue(typeof(T), out EventDelegate tempDel)) {
            this.del[typeof(T)] = tempDel + inDelegate; 
        } else this.del[typeof(T)] = inDelegate;

        return inDelegate;
    }

    public void AddListener<T> (EventDelegate<T> inputDel) where T : GameEvent {
        AddDelegate(inputDel);
    }

    public void AddListenerOnce<T> (EventDelegate<T> inputDel) where T : GameEvent {
        EventDelegate result = AddDelegate(inputDel);

        if(result != null) lookups[result] = inputDel;
    }

    public void RemoveListener<T> (EventDelegate<T> inputDel) where T : GameEvent
    {
        if (!delDict.TryGetValue(inputDel, out EventDelegate inDelegate)) return;
        if (del.TryGetValue(typeof(T), out EventDelegate tempDel)){
            tempDel -= inDelegate;
            if (tempDel == null) del.Remove(typeof(T));
            else del[typeof(T)] = tempDel;
        }

        delDict.Remove(inputDel);
    }

    void RemoveAll(){
        del.Clear();
        delDict.Clear();
        lookups.Clear();
    }

    /*public bool HasListener<T> (EventDelegate<T> del) where T : GameEvent {
        return delegateDictionary.ContainsKey(del);
    }*/

    void TriggerEvent (GameEvent e) {
        if (del.TryGetValue(e.GetType(), out EventDelegate newdel)) {
            newdel.Invoke(e);

            // remove listeners which should only be called once
            foreach(Delegate delInDict in del[e.GetType()].GetInvocationList())
            {
                EventDelegate key = (EventDelegate)delInDict;
                if (!lookups.ContainsKey(key)) continue;
                del[e.GetType()] -= key;

                if(del[e.GetType()] == null) del.Remove(e.GetType());

                delDict.Remove(lookups[key]);
                lookups.Remove(key);
            }
        } else {
            Debug.LogWarning("Event: " + e.GetType() + " has 0 listeners");
        }
    }
    
    public bool QueueEvent(GameEvent evt) {
        if (!del.ContainsKey(evt.GetType())) {
            return false;
        }

        mEventQueue.Enqueue(evt);
        return true;
    }
    
    void Update() {
        float timer = 0.0f;
        while (mEventQueue.Count > 0) {
            if (limitQueueProcessing) {
                if (timer > queueProcessTime)
                    return;
            }

            GameEvent ev = mEventQueue.Dequeue() as GameEvent;
            TriggerEvent(ev);

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