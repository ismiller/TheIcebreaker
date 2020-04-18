using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class CameraActor : Actor {

        [SerializeField] private CameraMotionComponent motionComponent;
        private static Transform mainCamera;
        private bool isFirstStart = true;

        public CameraMotionComponent MotionComponent { 
            get { return motionComponent; } 
        }

        public static Vector3 Right {
            get { return mainCamera.right; }
        }

        public static Vector3 Up {
            get { return mainCamera.up; }
        }

        private void Start() {
            mainCamera = Player;
            HandlerInitialize(motionComponent.GetHandler(this));
            UpdateManager.AddTo(this);
            isFirstStart = false;
        }

        protected override void OnEnable() {
            if (!isFirstStart) {
                base.OnEnable();
                UpdateManager.AddTo(this);
            }
        }

        protected override void OnDisable() {
            base.OnDisable();
            if (!Toolbox.isApplicationQuitting) 
                UpdateManager.RemoveFrom(this);;
        }

        public void SetPlayer(Transform _player) {
            motionComponent.GetHandler(this).RefreshPlayer(_player);
        }

        public void SetNewPoint(Vector3 _point, float _speed, float _time) {
            motionComponent.GetHandler(this).StartMovemetInNewPoint(_point, _speed, _time);
        }
    }
}
