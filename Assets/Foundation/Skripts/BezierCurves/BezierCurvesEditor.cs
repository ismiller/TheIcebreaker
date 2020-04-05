using UnityEngine;
using UnityEditor;

namespace Scaramouche.Game {
    #if UNITY_EDITOR
    [CustomEditor(typeof(BezierCurves))]
    public class BezierCurvesEditor : Editor {

        private Vector3[] lineDrawPoint;

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            BezierCurves e = (BezierCurves)target;

            GUILayout.Space(15);
            if (GUILayout.Button("Add New")) {
                e.AddPoint();
            }

            GUILayout.Space(5);
            if (GUILayout.Button("Destroy Last")) {
                e.DestroyLast();
            }

            GUILayout.Space(5);
            if (GUILayout.Button("Clear All")) {
                e.ClearAll();
            }
        }

        public void OnSceneGUI() {
            DrawBezierCurves();
        }

        private void DrawBezierCurves() {
            Handles.color = Color.green;
            BezierCurves e = (BezierCurves)target;
            if (e.bezierPath.Length == 0) {
                return;
            } else {
                lineDrawPoint = new Vector3[e.bezierPath.Length];
            }

            for(var i = 0; i < e.bezierPath.Length; i++) {
                lineDrawPoint[i] = e.bezierPath[i];
            }
            Handles.DrawPolyLine(lineDrawPoint);
        }
    }
    #endif
}
