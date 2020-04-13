using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class SlopeSurfaceActor : Actor, IAcceptVisitorRayCast {
        
        [SerializeField] private BezierCurves bezierCurvesSliding;

        private void Start() {
            ThisTransform = transform.GetComponent<Transform>();
        }

        public Vector3[] GetSlidingPath() {
            return bezierCurvesSliding.bezierPath;
        }

        public void AcceptDownCast(IVisitorEnvironment _visitor) {
            _visitor.Visit(this);
        }

        public void AcceptForwardCast(IVisitorEnvironment _visitor) {

        }
    }
}
