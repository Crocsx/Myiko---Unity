using UnityEngine;
using System;
using System.Collections.Generic;

public class UnityMainThreadDispatcher : Singleton<UnityMainThreadDispatcher>
{

    private static readonly Queue<Action> actions = new Queue<Action>();
    private static readonly object lockObject = new object();
    private const int maxActionsPerFrame = 10;

    private void Update()
    {
        int actionCount = 0;
        while (actions.Count > 0 && actionCount < maxActionsPerFrame)
        {
            Action action;
            lock (lockObject)
            {
                action = actions.Dequeue();
            }
            action();
            actionCount++;
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