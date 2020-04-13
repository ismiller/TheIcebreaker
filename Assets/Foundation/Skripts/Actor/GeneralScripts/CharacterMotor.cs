using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class CharacterMotor {
        
        private PlayerMotionHandler motionHandler;
        private PlayerMotionComponent motionComponent;
        private CharacterController characterController;
        private Transform player;
        //------------
        private Vector3 moveDirection;
        private float speed;
        private float currentSpeed;
        private float distanceTemp;
        private Vector2 currenDirection;
        //-------------

        public CharacterMotor(PlayerMotionHandler _motionHandler) {
            motionHandler = _motionHandler;
            motionComponent = motionHandler.MotionComponent;
            player = motionHandler.CharacterTransform;
            characterController = player.GetComponent<CharacterController>();
            currentSpeed = ComputeSpeedDependDirection(moveDirection);
        }

        public void MovementDirectional(Vector2 _direction) { 
            if (_direction != Vector2.zero) { 
                currenDirection = _direction; 
                moveDirection = ComputeDirectionDependCamera(currenDirection);
                currentSpeed = ComputeSpeedDependDirection(moveDirection);
            } else {
                moveDirection = ComputeDirectionDependCamera(currenDirection);
                currentSpeed = Mathf.MoveTowards(currentSpeed, .0f, .5f);
            }
            Move(moveDirection, currentSpeed, true);
        }

        public bool MovementDash(Vector2 _direction) {
            currenDirection = Vector2.zero;
            if (distanceTemp <= 0) { distanceTemp = motionComponent.DashTime; }
            moveDirection = ComputeDirectionDependCamera(_direction);
            distanceTemp -= motionComponent.DashStep;
            if (distanceTemp > 0) { 
                Move(moveDirection, motionComponent.DashSpeed, true);
                return true; 
            } else {
                distanceTemp = 0;
                return false;
            }
        }

        public bool JumpObstacle(Vector2 _direction) {
            currenDirection = Vector2.zero;
            if (distanceTemp <= 0) { distanceTemp = motionComponent.JumpTime; }
            moveDirection = new Vector3(_direction.x, 0, _direction.y);
            distanceTemp -= motionComponent.JumpStep;
            if(distanceTemp > 0) {
                Move(moveDirection, motionComponent.JumpSpeed, false);
                return true;
            } else {
                distanceTemp = 0;
                return false;
            }
        }

        public void MovementSlidingSlope(Vector2 _direction) {
            currenDirection = Vector2.zero;
            moveDirection = new Vector3(_direction.x, 0, _direction.y);
            Move(moveDirection, ComputeSlidingSpeed(), true);
        }

        public void RotateDirection(Vector3 _direction) {
            float speed = motionComponent.RotateSpeed * Time.deltaTime;
            Quaternion newRotation = Quaternion.Lerp(player.rotation, Quaternion.LookRotation(_direction), speed);
            player.rotation = new Quaternion(player.rotation.x, newRotation.y, player.rotation.z, newRotation.w);
        }

        public Vector3 SearchStartPointInArray(out int _numberPoint) {
            _numberPoint = 0;
            var currentPoint = motionHandler.PatchTemp[0];
            for (var i = 0; i < motionHandler.PatchTemp.Length - 1; i++) {
                var nextPoint = motionHandler.PatchTemp[i + 1];
                if (Vector3.Distance(player.position, nextPoint) < Vector3.Distance(player.position, currentPoint)) {
                    currentPoint = nextPoint;
                    _numberPoint = i + 1;
                }
            }
            return motionHandler.PatchTemp[_numberPoint];
        }

        public Vector2 ComputeDirectionDependPoint(Vector3 _point) {
            var heading = _point - player.position;
            var newDirection = heading / heading.magnitude;
            return new Vector2(newDirection.x, newDirection.z);
        }

        public Vector3 ComputeDirectionDependCamera(Vector2 _direction) {
            return ((_direction.y * CameraActor.Up) + (_direction.x * CameraActor.Right)).normalized;
        }

        public Vector3 ComputeDirectionDependSpeed(Vector3 _direction, float _speed) {
            return _direction * _speed * Time.deltaTime;
        }
        
        public float ComputeSlidingSpeed() {
            return motionHandler.IsBeginSlope ? motionComponent.SpeedIfBeginSlope : motionComponent.SpeedIfEndSlope;
        }

        public float ComputeSpeedDependDirection(Vector3 _direction) {
            float dot = Vector3.Dot(player.forward, _direction);
            if (dot >= motionComponent.ForvardAngleApp) {
                return motionComponent.ForvardSpeed;
            } else if (dot <= (-motionComponent.BackAngleApp)) {
                return motionComponent.BackSpeed;
            } else if (dot <= motionComponent.SideAngleApp || dot >= -motionComponent.SideAngleApp) {
                return motionComponent.SideSpeed;
            } else return motionComponent.ForvardSpeed;
        }

        public Vector3 ApplyGravity(Vector3 _direction) {
            if(!motionHandler.IsGraund) { _direction.y = -motionComponent.GravitySpeed * 2.5f * Time.deltaTime;             
            } else { _direction.y = -1.0f; }
            return _direction;
        }

        private void Move(Vector3 _direction, float _speed, bool _isGravity) {
            _direction = ComputeDirectionDependSpeed(_direction, _speed);
            if (_isGravity) { _direction = ApplyGravity(_direction); }
            characterController.Move(Vector3.ClampMagnitude(_direction, _speed));
        }
    }
}
