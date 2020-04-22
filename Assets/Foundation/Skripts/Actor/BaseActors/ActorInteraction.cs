using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class ActorInteraction : Actor {
        
        protected override void Start() {
            layer = 10;
            base.Start();
        } 

        protected IInterplayInterface interact;
        protected bool isInteract => interact != null;

        protected virtual void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<ActorCharacter>(out ActorCharacter actor)) {
                if (actor.TryGetInterplayComponent(ref interact)) { /* */ }
            }
        }

        protected virtual void OnTriggerStay(Collider other) { /* */ }

        protected virtual void OnTriggerExit(Collider other) {
            if (isInteract) { interact = null; }
        }
    }
}
