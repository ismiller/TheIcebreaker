using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class CameraActor : ActorCharacter {

        [SerializeField] private CameraMotionComponent motionComponent;
        private static Transform mainCamera;

        public CameraMotionComponent MotionComponent { 
            get { return motionComponent; } 
        }

        public static Vector3 Right {
            get { return mainCamera.right; }
        }

        public static Vector3 Up {
            get { return mainCamera.up; }
        }

        protected override void Start() {
            mainCamera = Player;
            HandlerInitialize(motionComponent.GetHandler(this));
            UpdateManager.AddTo(this);
            base.Start();
        }

        protected override void OnEnable() => base.OnEnable();
        protected override void OnDisable() => base.OnDisable();

        public void SetPlayer(Transform _player) {
            motionComponent.GetHandler(this).RefreshPlayer(_player);
        }

        public void SetNewPoint(Vector3 _point, float _speed, float _time) {
            motionComponent.GetHandler(this).StartMovemetInNewPoint(_point, _speed, _time);
        }
    }
}
