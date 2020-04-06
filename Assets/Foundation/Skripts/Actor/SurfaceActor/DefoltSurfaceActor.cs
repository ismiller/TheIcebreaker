using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class DefoltSurfaceActor : Actor, IAcceptVisitor {

        private void Start() {
            ThisTransform = transform.GetComponent<Transform>();
        }

        public void AcceptRayCast(IVisitorEnvironment _visitor) {
            _visitor.Visit(this);
        }

        public void AcceptOnTrigger(IVisitorEnvironment _visitor) {
            return;
        }
    }
}
