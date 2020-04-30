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
            rotateDirection = (motionHandler.GetObstaclePoint - dataBox.GetTransform.position).normalized;
            direction = new Vector2(rotateDirection.x, rotateDirection.z);
            jumpTask = Task.CreateTask(Jump());
            startJump = stopJump = false;
            dataBox.GetCHController.height = 1;
            dataBox.GetCHController.center = new Vector3(0, 0.7f, 0);
            dataBox.GetAnimator.SetTrigger("JumpObstacle");
        }

        public void LogicUpdate() {
            if (stopJump) { 
                dataBox.GetStateMachine.ChangeMovementState(dataBox.GetStateBox.GetValkState); 
            }
        }

        public void PhisicUpdate() {
            dataBox.GetCharacterMotor.RotateDirection(rotateDirection);
            if (!startJump) {
                startJump = true;
                jumpTask.Start();
            }
        }

        public void Exit() {
            jumpTask = null;
            dataBox.GetCHController.height = 2;
            dataBox.GetCHController.center = new Vector3(0, 0, 0);
        }

        private IEnumerator Jump() {
            while(!stopJump) {
                yield return new WaitForEndOfFrame();
                if (!dataBox.GetCharacterMotor.JumpObstacle(direction)) {
                    stopJump = true;
                }
            }
        }
    }
}
