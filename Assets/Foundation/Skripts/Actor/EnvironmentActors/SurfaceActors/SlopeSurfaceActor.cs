using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class SlopeSurfaceActor : ActorSurface, IAcceptRayCast {
        
        [SerializeField] private BezierCurves bezierCurvesSliding;

        protected override void Start() {
            base.Start();
        }

        public Vector3[] GetSlidingPath() {
            return bezierCurvesSliding.bezierPath;
        }

        public void Accept(IVisitor _visitor) {
            _visitor.Visit(this);
        }

    }
}
