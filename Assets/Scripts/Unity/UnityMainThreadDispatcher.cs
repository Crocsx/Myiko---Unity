using UnityEngine;
using System;
using System.Collections.Generic;

public class UnityMainThreadDispatcher : Singleton<UnityMainThreadDispatcher>
{

    private static readonly Queue<Action> actions = new Queue<Action>();
    private static readonly object lockObject = new object();


    private void Update()
    {
        while (actions.Count > 0)
        {
            Action action;
            lock (lockObject)
            {
                action = actions.Dequeue();
            }
            action();
        }
    }

    public void Enqueue(Action action)
    {
        lock (lockObject)
        {
            actions.Enqueue(action);
        }
    }


}