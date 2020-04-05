using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class SlidingActor : Actor {

        private void Start() {
            ThisTransform = GetComponent<Transform>();
        }
    }
}
