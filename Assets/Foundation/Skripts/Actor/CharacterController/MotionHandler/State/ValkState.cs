using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class ValkState : BaseMovementState, IMovement {

        private int slidingPoint;
        private float currentSpeed;

        public ValkState(PlayerMotionHandler _motionHandler) : base(_motionHandler) {
            InputManager.GetVectorMove += (_value) => direction = _value;
            dashPossible = true;
        }

        public void Enter() {
            InputManager.GetMousePoint += (Vector3 _value) => rotateDirection = _value;
            currentSpeed = motionComponent.ForvardSpeed;
            InputManager.GetKeyDash += SetInDash;
        }

        public void LogicUpdate() {
            if (motionHandler.IsSlope) { motionStateMachine.ChangeMovementState(motionHandler.GetMoveStateBox.GetSlidingSlopeState); }
        }

        public void PhisicUpdate() {
            characterMotor.RotateDirection(rotateDirection - player.position);
            characterMotor.MovementDirectional(direction);
        }

        public void Exit() {
            InputManager.GetMousePoint -= (Vector3 _value) => direction = _value;
            InputManager.GetKeyDash -= SetInDash;
        }

        private void SetInDash(bool _value) {
            if (motionHandler.IsObstacle) {
                motionStateMachine.ChangeMovementState(motionHandler.GetMoveStateBox.GetJumpObstacleState);
            } else {
                if ((direction != Vector2.zero) && dashPossible) {
                    motionStateMachine.ChangeMovementState(motionHandler.GetMoveStateBox.GetDashState.Start(direction));
                    Task.CreateTask(DashReturn()).Start();
                }
            }
        }

        private IEnumerator DashReturn() {
            dashPossible = false;
            yield return new WaitForSeconds(motionComponent.ReturnTimeDash);
            dashPossible = true;
        }
    }
}