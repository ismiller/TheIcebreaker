using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class BaseMainHandler {
        
        protected Transform characterTransform;
        public Transform CharacterTransform { get { return characterTransform; } }

        public BaseMainHandler() {

        }

        public virtual void Initialize(Actor _actor){
            characterTransform = _actor.Player;
        }

        public abstract void StartHandle();
        public abstract void StopHandle();
    }
}
