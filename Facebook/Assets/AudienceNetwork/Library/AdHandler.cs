using UnityEngine;
using System;
using System.Collections.Generic;

namespace AudienceNetwork
{
    public class AdHandler : MonoBehaviour
    {
        private static readonly Queue<Action> executeOnMainThreadQueue = new Queue<Action>();

        public void ExecuteOnMainThread(Action action)
        {
            lock (executeOnMainThreadQueue)
            {
                executeOnMainThreadQueue.Enqueue(action);
            }
        }

        void Update()
        {
            // dispatch stuff on main thread
            while (executeOnMainThreadQueue.Count > 0)
            {
                Action dequeuedAction = null;
                lock (executeOnMainThreadQueue)
                {
                    try
                    {
                        dequeuedAction = executeOnMainThreadQueue.Dequeue();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                if (dequeuedAction != null)
                {
                    dequeuedAction.Invoke();
                }
            }
        }

        public void RemoveFromParent()
        {
#if !UNITY_EDITOR
    UnityEngine.Object.Destroy(this);
#endif
        }
    }
}
