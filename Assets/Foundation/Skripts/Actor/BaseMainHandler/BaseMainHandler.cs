using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class BaseMainHandler {

        private Actor actor;
        protected Transform player => actor.Player;

        public BaseMainHandler() { }

        public virtual void Initialize(Actor _actor){
            actor = _actor;
        }

        public abstract void StartHandle();
        public abstract void StopHandle();
    }
}
