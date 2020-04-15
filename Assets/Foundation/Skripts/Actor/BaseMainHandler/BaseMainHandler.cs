using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class BaseMainHandler {
        
        protected Transform characterTransform;
        public Transform CharacterTransform { get { return characterTransform; } }

        public BaseMainHandler(Actor _actor) {
            characterTransform = _actor.Player;
            UpdateManager.AddTo(this);
        }
    }
}
