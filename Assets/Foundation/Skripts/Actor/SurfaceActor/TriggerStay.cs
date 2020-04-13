using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class TriggerStay : Actor {
        
        private void Start() {
            ThisTransform = transform.GetComponent<Transform>();
        }


    }
}
