using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class SlipperySurfaceActor : Actor, IAcceptVisitor {

        private void Start() {
            ThisTransform = GetComponent<Transform>();
        }

        public void AcceptRayCast(IVisitor _visitor) {
            _visitor.Visit(this);
        }

        public void AcceptOnTrigger(IVisitor _visitor) {
            return;
        }
    }
}
