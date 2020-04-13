using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class JumpObstacleState : BaseMovementState, IMovement {

        private CharacterController controller;
        private ITask jumpTask;
        private bool startJump;
        private bool stopJump;

        public JumpObstacleState(PlayerMotionHandler _motionHandler) : base(motionHandler) {
            controller = _motionHandler.CharacterTransform.GetComponent<CharacterController>();
        }

        public void Enter() {
            rotateDirection = (motionHandler.ObstaclePoint - player.position).normalized;
            direction = new Vector2(rotateDirection.x, rotateDirection.z);
            jumpTask = Task.CreateTask(Jump());
            startJump = stopJump = false;
            controller.height = 1;
            controller.center = new Vector3(0, 0.7f, 0);
            playerAnimator.SetTrigger("JumpObstacle");
        }

        public void LogicUpdate() {
            if (stopJump) { 
                motionStateMachine.ChangeMovementState(motionHandler.valkState); 
            }
        }

        public void PhisicUpdate() {
            characterMotor.RotateDirection(rotateDirection);
            if (!startJump) {
                startJump = true;
                jumpTask.Start();
            }
        }

        public void Exit() {
            jumpTask = null;
            controller.height = 2;
            controller.center = new Vector3(0, 0, 0);
        }

        private IEnumerator Jump() {
            while(!stopJump) {
                yield return null;
                if (!characterMotor.JumpObstacle(direction)) {
                    stopJump = true;
                }
            }
        }
    }
}
