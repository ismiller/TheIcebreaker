using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class SlidingSlopeState : BaseMovementState, IMovement {

        private Vector3 targetPoint;
        private int nextPoint;
        private float currentSpeed;
        private bool endSlope;

        public SlidingSlopeState(PlayerMotionHandler _motionHandler) : base(_motionHandler) {

        }

        public void Enter() {
            endSlope = false;
            rotateDirection = targetPoint = characterMotor.SearchStartPointInArray(out nextPoint);
            direction = characterMotor.ComputeDirectionDependPoint(targetPoint);  
            currentSpeed = characterMotor.ComputeSlidingSpeed();   
            playerAnimator.SetBool("isSliding", true);
        }

        public void LogicUpdate() {

        }

        public void PhisicUpdate() {
            if(Vector3.Distance(player.position, targetPoint) < 1) {
                if ((++nextPoint) < motionHandler.GetPatchTemp.Length) {
                    rotateDirection = targetPoint = motionHandler.GetPatchTemp[nextPoint];
                    direction = characterMotor.ComputeDirectionDependPoint(targetPoint);
                    rotateDirection -= player.position;
                } else if (!endSlope) { 
                    rotateDirection = rotateDirection.normalized; 
                    Task.CreateTask(EndSliding()).Start();
                    endSlope = true;
                }
            } 
            if (rotateDirection != Vector3.zero) {
                characterMotor.RotateDirection(rotateDirection); 
            }
            characterMotor.MovementSlidingSlope(direction);
        }

        public void Exit() {
            playerAnimator.SetBool("isSliding", false);
        }

        private IEnumerator EndSliding() {
            currentSpeed *= 0.5f; 
            while (currentSpeed > 0) {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, 0.2f);
                yield return null;
            }
            motionStateMachine.ChangeMovementState(motionHandler.GetMoveStateBox.GetValkState);
        }
    }
}
