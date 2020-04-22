using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class ControlComponent<T> : ScriptableObject where T : BaseMainHandler, new() {
        
        private T handler;

        public T GetHandler(Actor _actor) { 
            if (handler == null) {
                handler = new T();
                handler.Initialize(_actor);
            }
            return handler;
        }
    }
}
