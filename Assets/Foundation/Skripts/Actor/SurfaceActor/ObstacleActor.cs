using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class ObstacleActor : Actor, IAcceptVisitorRayCast {
        
        public void Start() {
            ThisTransform = transform.GetComponent<Transform>();
        }

        public void AcceptDownCast(IVisitorEnvironment _visitor) {

        }

        public void AcceptForwardCast(IVisitorEnvironment _visitor) {
            _visitor.Visit(this);
        }
    }
}
