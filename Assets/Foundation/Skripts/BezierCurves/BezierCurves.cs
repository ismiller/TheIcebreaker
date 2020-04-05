using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    [ExecuteInEditMode]
    public class BezierCurves : MonoBehaviour {

        public BezierCurvesActor prefab;
        public Vector3 offset; // смещение следующей точки
        public BezierCurvesActor[] point;
        public Vector3[] bezierPath; // конечный результат
        public int segmentCount = 25; // количество точек, между соседними А и Б (чем больше, тем сильнее сглаживание и выше нагрузка на систему)

        private BezierCurvesActor last;
        private Vector3 lastPos;
        private int id;

        public void AddPoint() {
            point = new BezierCurvesActor[transform.childCount + 1];

            for (int j = 0; j < transform.childCount; j++) {
                point[j] = transform.GetChild(j).GetComponent<BezierCurvesActor>();
            }

            if (last) lastPos = last.transform.position;
            last = Instantiate(prefab) as BezierCurvesActor;
            last.gameObject.name = "BezierPoint_" + id;
            if (point.Length > 1) last.transform.position = lastPos + offset; else last.transform.position = transform.position;
            last.transform.parent = transform;
            point[point.Length - 1] = last;

            id++;
        }

        public void ClearAll() {
            for (int i = 0; i < point.Length; i++) {
                if (point[i]) DestroyImmediate(point[i].gameObject);
            }

            point = new BezierCurvesActor[0];
            id = 0;
            DrawCurves();
        }

        public void DestroyLast() {

            if (last == null) return;

            DestroyImmediate(last.gameObject);

            lastPos -= offset;

            point = new BezierCurvesActor[transform.childCount];

            for (int j = 0; j < transform.childCount; j++) {
                point[j] = transform.GetChild(j).GetComponent<BezierCurvesActor>();
                last = point[j];
            }

            id--;
            DrawCurves();
        }

        #if UNITY_EDITOR
        private void Update() {
            if (point.Length < 2 || segmentCount < 6) return;
            DrawCurves();
        }
        #endif

        public void DrawCurves() {
            bool sw = true;
            List<Vector3> p = new List<Vector3>();
            List<Vector3> l = new List<Vector3>();
            for (int i = 1; i < point.Length; i++) {
                if (p.Count == 0) {
                    p.Add(point[i-1].endPoint.position);
                    p.Add(point[i-1].adjustPoint.position);
                    p.Add(point[i].adjustPoint.position);
                } else if (!sw) {
                    p.Add(point[i-1].adjustMirror.position);
                    p.Add(point[i].adjustMirror.position);
                } else {
                    p.Add(point[i-1].adjustPoint.position);
                    p.Add(point[i].adjustPoint.position);
                }

                p.Add(point[i].endPoint.position);
                sw = !sw;
            }

            for (int i = 0; i < p.Count - 3; i += 3) {
                if (l.Count > 0) l.RemoveAt(l.Count - 1); 
                for (int j = 0; j <= segmentCount; j++) {
                    float t = (float)j/segmentCount;
                    Vector3 pxl = CalculateBezierPoint(t, p[i], p[i + 1], p[i + 2], p[i + 3]);
                    l.Add(pxl);
                }
            }

            bezierPath = new Vector3[] {};
            bezierPath = l.ToArray();
        }
        
        Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;
            return p;
        }
    }
}
