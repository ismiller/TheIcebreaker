using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class RemoteControlActor : ActorInteraction {
        
        [SerializeField] private MovePlatformActor platform;

        public void Use() {
            platform.Activate();
        }

        protected override void OnTriggerEnter(Collider other) {
            base.OnTriggerEnter(other);
            if (isInteract) {
                interact.TriggerEnter(this);
            }
        }

        protected override void OnTriggerStay(Collider other) {
            base.OnTriggerEnter(other);
            if (isInteract) {
                interact.TriggerStay(this);
            }
        }

        protected override void OnTriggerExit(Collider other) {
            if (isInteract) {
                interact.TriggerEnter(this);
            }
            base.OnTriggerExit(other);
        }
    }
}
