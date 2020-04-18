using System.Collections;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class Task : ITask {

        private MonoBehaviour host;
        private Coroutine coroutine;
        private Action feedback;
        private IEnumerator taskAction;

        private bool highPriority;
        public bool HighPriority { get; }

        private bool isProcessing => coroutine != null;
        private bool isFeedback => feedback != null;

        public static ITask CreateTask(IEnumerator _action, bool _highPriority = false) {
            return new Task(_action, _highPriority);
        }

        public Task(IEnumerator _action, bool _highPriority = false) {
            host = TaskManager.host;
            taskAction = _action;
            highPriority = _highPriority;
        }

        public void Start() {
            Stop();
            coroutine = host.StartCoroutine(RunTask());
        }

        public void Stop() {
            if(isProcessing) {
                if (!MonoHostComponent.isApplicationQuitting) {
                    host.StopCoroutine(coroutine);
                    coroutine = null;
                }
            }
        }

        public ITask Subscrible(Action _feedbeck = null) {
            feedback += _feedbeck;
            return this;
        }

        private void CallSubscrible() {
            if(isFeedback) {
                feedback();
            }
        }

        private IEnumerator RunTask() {
            yield return taskAction;
            CallSubscrible();
            coroutine = null;
        }    
    }
}
