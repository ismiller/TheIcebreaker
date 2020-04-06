using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class SlopeSurfaceActor : Actor, IAcceptVisitor {
        
        [SerializeField] private BezierCurves bezierCurvesSliding;

        private void Start() {
            ThisTransform = transform.GetComponent<Transform>();
        }

        public Vector3[] GetSlidingPath() {
            return bezierCurvesSliding.bezierPath;
        }

        public void AcceptRayCast(IVisitor _visitor) {
            _visitor.Visit(this);
        }

        public void AcceptOnTrigger(IVisitor _visitor) {
            return;
        }
    }
}
