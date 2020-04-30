using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class ValkState : BaseMovementState, IMovement {

        private float currentSpeed;

        public ValkState(PlayerMotionHandler _motionHandler) : base(_motionHandler) {
            InputManager.GetVectorMove += (_value) => direction = _value;
            dashPossible = true;
        }

        public void Enter() {
            InputManager.GetMousePoint += (Vector3 _value) => rotateDirection = _value;
            currentSpeed = dataBox.GetMotionComponent.ForvardSpeed;
            InputManager.GetKeyDash += SetInDash;
        }

        public void LogicUpdate() {
            if (motionHandler.IsSlope) { dataBox.GetStateMachine.ChangeMovementState(dataBox.GetStateBox.GetSlidingSlopeState); }
        }

        public void PhisicUpdate() {
            dataBox.GetCharacterMotor.RotateDirection(rotateDirection - dataBox.GetTransform.position);
            dataBox.GetCharacterMotor.MovementDirectional(direction);
        }

        public void Exit() {
            InputManager.GetMousePoint -= (Vector3 _value) => direction = _value;
            InputManager.GetKeyDash -= SetInDash;
        }

        private void SetInDash(bool _value) {
            if (motionHandler.IsObstacle) {
                dataBox.GetStateMachine.ChangeMovementState(dataBox.GetStateBox.GetJumpObstacleState);
            } else {
                if ((direction != Vector2.zero) && dashPossible) {
                    dataBox.GetStateMachine.ChangeMovementState(dataBox.GetStateBox.GetDashState.Start(direction));
                    Task.CreateTask(DashReturn()).Start();
                }
            }
        }

        private IEnumerator DashReturn() {
            dashPossible = false;
            yield return new WaitForSeconds(dataBox.GetMotionComponent.ReturnTimeDash);
            dashPossible = true;
        }
    }
}