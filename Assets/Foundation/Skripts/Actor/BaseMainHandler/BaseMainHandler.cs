using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class BaseMainHandler {

        protected Transform player;

        public BaseMainHandler() { }

        public virtual void Initialize(ActorCharacter _actor){
            player = _actor.Player;
        }

        public abstract void StartHandle();
        public abstract void StopHandle();
    }
}
