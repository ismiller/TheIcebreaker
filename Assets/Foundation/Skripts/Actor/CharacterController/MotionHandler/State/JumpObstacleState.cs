using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class JumpObstacleState : BaseMovementState, IMovement {

        private ITask jumpTask;
        private bool startJump;
        private bool stopJump;

        public JumpObstacleState(PlayerMotionHandler _motionHandler) : base(_motionHandler) {

        }

        public void Enter() {
            rotateDirection = (motionHandler.GetObstaclePoint - player.position).normalized;
            direction = new Vector2(rotateDirection.x, rotateDirection.z);
            jumpTask = Task.CreateTask(Jump());
            startJump = stopJump = false;
            chController.height = 1;
            chController.center = new Vector3(0, 0.7f, 0);
            playerAnimator.SetTrigger("JumpObstacle");
        }

        public void LogicUpdate() {
            if (stopJump) { 
                motionStateMachine.ChangeMovementState(motionHandler.GetMoveStateBox.GetValkState); 
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
            chController.height = 2;
            chController.center = new Vector3(0, 0, 0);
        }

        private IEnumerator Jump() {
            while(!stopJump) {
                yield return new WaitForEndOfFrame();
                if (!characterMotor.JumpObstacle(direction)) {
                    stopJump = true;
                }
            }
        }
    }
}
