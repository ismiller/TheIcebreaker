using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class MediatorController : BaseMainHandler{
        
        private Dictionary<Type, Delegate> subscribers = new Dictionary<Type, Delegate>();
        
        public void AddSubscribe<T>(MediatorCallback<T> _message) where T : Message {
            if (_message == null) return;
            var t = typeof(T);
            if (subscribers.ContainsKey(t)) {
                subscribers[t] = Delegate.Combine(subscribers[t], _message); 
            } else {
                subscribers.Add(t, _message);
            }
        }

        public void RemoveSubscribe<T>(MediatorCallback<T> _message) where T : Message {
            if (_message == null) return;
            var t = typeof(T);
            if (subscribers.ContainsKey(t)) {
                var temp = subscribers[t];
                temp = System.Delegate.Remove(temp, _message);
                if (temp == null) { subscribers.Remove(t); }
                else { subscribers[t] = temp; }
            }
        }

        public void SendMessage<T>(T _message) where T : Message {
            var t = typeof(T);
            if (subscribers.ContainsKey(t)) {
                subscribers[t].DynamicInvoke(_message);
            }
        }

        public override void StartHandle() { /* */ }

        public override void StopHandle() {/* */ }
    }
}
