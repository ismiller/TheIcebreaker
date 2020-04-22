using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class ActorSurface : Actor {

        protected override void Start() {
            layer = 8;
            base.Start();
        }
    }
}
