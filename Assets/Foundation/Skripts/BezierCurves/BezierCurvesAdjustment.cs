using UnityEngine;
using System.Collections;

namespace Scaramouche.Game {
    [ExecuteInEditMode]
    public class BezierCurvesAdjustment : Actor {

        public Transform mirror, parent;
        public Color color = Color.green;
        public float scale = 0.5f;

        void OnDrawGizmos() {
            Gizmos.color = color;
            Gizmos.DrawCube(Player.position, Vector3.one * scale);
            Gizmos.DrawSphere(mirror.position, scale/2);
            Gizmos.DrawLine(Player.position, mirror.position);
        }

        #if UNITY_EDITOR
        void LateUpdate() {
            mirror.position = parent.position + (Player.localPosition * -1);
        }
        #endif
    }
}
