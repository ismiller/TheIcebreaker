using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scaramouche.Game {
    public class CameraMotionHandler : BaseMainHandler, IUpdateHandler, ILateUpdateHandler {

        private CameraActor cameraActor;
        private CameraMotionComponent motionComponent => cameraActor.MotionComponent;
        private Transform camera => cameraActor.Player;
        private Vector3 currentPointView, lastPointView;
        private float valueTurn, valueKeyTurn;
        private Vector2 cameraRotate;
        private bool isMoveInPoint;
        private Transform player;
        //------------
        private float screenWidth;
        private float leftTurnArea;
        private float rightTurnArea;
        private ITask calculateArea;
        
        private ITask GetCalculateArea {
            get {
                if (calculateArea == null) {
                    calculateArea = Task.CreateTask(CalculateArea());
                }
                return calculateArea;
            }
        }

        public CameraMotionHandler() : base() { }

        public override void Initialize(Actor _actor) {
            base.Initialize(_actor);
            cameraActor = (CameraActor)_actor;
            cameraRotate = new Vector2(camera.eulerAngles.x, camera.eulerAngles.y);
        }

        public override void StartHandle() {
            GetCalculateArea.Start();
            InputManager.GetKeyRotate += _value => valueKeyTurn = _value;
        }

        public override void StopHandle() {
            GetCalculateArea.Stop();
            InputManager.GetKeyRotate -= _value => valueKeyTurn = _value;
        }

        public void UpdateHandler() {
            if (valueKeyTurn == 0) { 
                CalculateMouseTurnValue(); 
            } else { 
                valueTurn = valueKeyTurn; 
            }
        }

        public void LateUpdateHandler() {
            SetPointView();
            if (motionComponent.ISRotate) { 
                Rotation(); 
            }
            Movement();
        }

        private void Rotation() {
            cameraRotate.y += valueTurn * motionComponent.Sensitivity;
            Quaternion newRotation = Quaternion.Euler(cameraRotate.x, cameraRotate.y, 0);
            camera.rotation = newRotation;
        }

        private void Movement() {
            
            if (motionComponent.IsFollow && (!isMoveInPoint)) {
                Vector3 newPposition = CalculateNewPosition(currentPointView);
                camera.position = newPposition;
            }
        }

        private bool SmoothMovement(float _speed) {
            Vector3 newPosition = CalculateNewPosition(currentPointView);
            if (motionComponent.IsFollow && isMoveInPoint) {
                camera.position = Vector3.MoveTowards(camera.position, newPosition, _speed * Time.deltaTime);
            }
            if (camera.position != newPosition) { 
                return true; 
            } else { 
                return false; 
            }
        }

        private Vector3 CalculateNewPosition(Vector3 _pointView) {
            return camera.rotation * new Vector3(0, 0, -motionComponent.Offset) + _pointView;
        }

        private void CalculateMouseTurnValue() {
            float mousePositionX = Mouse.current.position.ReadValue().x;
            if (mousePositionX < leftTurnArea) {
                valueTurn = -1;
            } else if (mousePositionX > rightTurnArea) {
                valueTurn = 1;
            } else { 
                valueTurn = valueKeyTurn; 
            }
        }

        private void SetPointView() {
            if (player && (!isMoveInPoint) ) { 
                SetPointView(player.position);
            }
        }

        private void SetPointView(Vector3 _newPoint) {
            currentPointView = _newPoint;
        }

        private void CalculateAreaMouseTurn() {
            screenWidth = Screen.width;
            var area = (motionComponent.AreaTurnMouse / 100) * screenWidth;
            leftTurnArea = area;
            rightTurnArea = screenWidth - area;
        }

        public void RefreshPlayer(Transform _player) {
            player = _player;
        }

        public void StartMovemetInNewPoint(Vector3 _point, float _speed, float _delayReturn) {
            lastPointView = currentPointView;
            SetPointView(_point);
            isMoveInPoint = true;
            TaskManager.AddTask(ReturnLastPoint(_speed, _delayReturn));
        }

        private IEnumerator ReturnLastPoint(float _speed, float _time) {
            while (SmoothMovement(_speed)) {
                yield return null;
            }
            yield return new WaitForSeconds(_time);
            SetPointView(lastPointView);
            while (SmoothMovement(_speed)) {
                yield return null;
            }
            isMoveInPoint = false;
        }

        private IEnumerator CalculateArea() {
            while (true) {
                yield return new WaitForSeconds(1);
                if (screenWidth != Screen.width) {
                    CalculateAreaMouseTurn();
                }
            }
        }

    }
}
