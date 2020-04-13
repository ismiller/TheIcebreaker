using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class DefoltSurfaceActor : Actor, IAcceptVisitorRayCast {

        private void Start() {
            ThisTransform = transform.GetComponent<Transform>();
        }

        public void AcceptDownCast(IVisitorEnvironment _visitor) {
            _visitor.Visit(this);
        }

        public void AcceptForwardCast(IVisitorEnvironment _visitor) {

        }
    }
}
