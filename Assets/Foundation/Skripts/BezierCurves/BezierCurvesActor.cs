using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class BezierCurvesActor : Actor {
        
        public Transform adjustPoint, endPoint, adjustMirror;
        public Color color = Color.black;
        public float scale = 0.5f;

        private void OnDrawGizmos() {
            Gizmos.color = color;
            Gizmos.DrawCube(ThisTransform.position, Vector3.one * scale);
        }
    }
}
