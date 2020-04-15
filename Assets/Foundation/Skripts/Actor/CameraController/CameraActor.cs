using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class CameraActor : Actor {

        [SerializeField] private CameraMotionComponent motionComponent;
        public CameraMotionComponent MotionComponent { get { return motionComponent; } }

        private static Transform mainCamera;

        public static Vector3 Right {
            get { return mainCamera.right; }
        }

        public static Vector3 Up {
            get { return mainCamera.up; }
        }

        private void Start() {
            mainCamera = Player;
            motionComponent.Initialize(Player);
        }

        public void SetPlayer(Transform _player) {
            motionComponent.MainHandler.RefreshPlayer(_player);
        }

        public void SetNewPoint(Vector3 _point, float _speed, float _time) {
            motionComponent.MainHandler.StartMovemetInNewPoint(_point, _speed, _time);
        }
    }
}
