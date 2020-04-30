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
            rotateDirection = targetPoint = dataBox.GetCharacterMotor.SearchStartPointInArray(out nextPoint);
            direction = dataBox.GetCharacterMotor.ComputeDirectionDependPoint(targetPoint);  
            currentSpeed = dataBox.GetCharacterMotor.ComputeSlidingSpeed();   
            dataBox.GetAnimator.SetBool("isSliding", true);
        }

        public void LogicUpdate() {

        }

        public void PhisicUpdate() {
            if(Vector3.Distance(dataBox.GetTransform.position, targetPoint) < 1) {
                if ((++nextPoint) < motionHandler.GetPatchTemp.Length) {
                    rotateDirection = targetPoint = motionHandler.GetPatchTemp[nextPoint];
                    direction = dataBox.GetCharacterMotor.ComputeDirectionDependPoint(targetPoint);
                    rotateDirection -= dataBox.GetTransform.position;
                } else if (!endSlope) { 
                    rotateDirection = rotateDirection.normalized; 
                    Task.CreateTask(EndSliding()).Start();
                    endSlope = true;
                }
            } 
            if (rotateDirection != Vector3.zero) {
                dataBox.GetCharacterMotor.RotateDirection(rotateDirection); 
            }
            dataBox.GetCharacterMotor.MovementSlidingSlope(direction);
        }

        public void Exit() {
            dataBox.GetAnimator.SetBool("isSliding", false);
        }

        private IEnumerator EndSliding() {
            currentSpeed *= 0.5f; 
            while (currentSpeed > 0) {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, 0.2f);
                yield return null;
            }
            dataBox.GetStateMachine.ChangeMovementState(dataBox.GetStateBox.GetValkState);
        }
    }
}
