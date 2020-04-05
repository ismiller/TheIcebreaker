using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class CameraActor : Actor {

        [SerializeField] private CameraMotionComponent motionComponent;
        public CameraMotionComponent MotionComponent { get { return motionComponent; } }

        private static Transform thisTransform;

        public static Vector3 Right {
            get { return thisTransform.right; }
        }

        public static Vector3 Up {
            get { return thisTransform.up; }
        }

        private void Start() {
            base.ThisTransform = thisTransform = transform.GetComponent<Transform>();
            motionComponent.Initialize(thisTransform);
        }

        public void SetPlayer(Transform _player) {
            motionComponent.MainHandler.RefreshPlayer(_player);
        }

        public void SetNewPoint(Vector3 _point, float _speed, float _time) {
            motionComponent.MainHandler.StartMovemetInNewPoint(_point, _speed, _time);
        }
    }
}
