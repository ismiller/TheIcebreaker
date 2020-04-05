using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class ObstacleActor : Actor, IAcceptVisitor {
        
        [SerializeField] private BezierCurves patch;

        public Vector3[] GetSlidingPath() {
            return patch.bezierPath;
        }

        public void Start() {
            ThisTransform = transform.GetComponent<Transform>();
        }

        public void AcceptRayCast(IVisitor _visitor) {
            _visitor.Visit(this);
        }

        public void AcceptOnTrigger(IVisitor _visitor) {
            _visitor.Visit(this);
        }
    }
}
