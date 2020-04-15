using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scaramouche.Game {
    public class CameraMotionHandler : BaseMainHandler, ITick, ITickLate {

        private CameraMotionComponent motionComponent;
        //------------
        private Transform player;
        private Transform camera;
        private Vector3 currentPointView;
        private Vector3 lastPointView;
        private Vector2 cameraRotate;
        private bool isMoveInPoint;
        private float valueTurn;
        private float valueKeyTurn;
        //------------
        private float screenWidth;
        private float leftTurnArea;
        private float rightTurnArea;

        public CameraMotionHandler(CameraActor _cameraActor) : base(_cameraActor) {
            camera = _cameraActor.Player;
            motionComponent = _cameraActor.MotionComponent;
            cameraRotate = new Vector2(camera.eulerAngles.x, camera.eulerAngles.y);
            InputManager.GetKeyRotate += _value => valueKeyTurn = _value;
            UpdateManager.AddTo(this);
            Task.CreateTask(CalculateArea()).Start();
        }

        public void Tick() {
            if (valueKeyTurn == 0) { CalculateMouseTurnValue(); }
            else { valueTurn = valueKeyTurn; }
        }

        public void TickLate() {
            SetPointView();
            if ((motionComponent.ISRotate) && (valueTurn != 0)) { Rotation(); }
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
            if (camera.position != newPosition) { return true; }
            else { return false; }
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
            } else { valueTurn = valueKeyTurn; }
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
