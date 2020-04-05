using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class SlidingSlopeState : BaseMovementState, IMovement {

        private Vector3 targetPoint;
        private int nextPoint;
        private float currentSpeed;

        public SlidingSlopeState(PlayerMotionHandler _motionHandler) : base(_motionHandler) {

        }

        public void Enter() {
            targetPoint = characterMotor.SearchStartPointInArray(out nextPoint);
            direction = characterMotor.ComputeDirectionDependPoint(targetPoint);  
            currentSpeed = characterMotor.ComputeSlidingSpeed();   
        }

        public void LogicUpdate() {
            if (!motionHandler.IsSlope) { Task.CreateTask(EndSliding()).Start(); }
        }

        public void PhisicUpdate() {
            if(Vector3.Distance(player.position, targetPoint) < 1) {
                if ((++nextPoint) < motionHandler.PatchTemp.Length) {
                    targetPoint = motionHandler.PatchTemp[nextPoint];
                    direction = characterMotor.ComputeDirectionDependPoint(targetPoint);
                }
            } 
            characterMotor.MovementSlidingSlope(direction);
        }

        public void Exit() {
            
        }

        private IEnumerator EndSliding() {
            while (currentSpeed > 0) {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, Time.deltaTime);
                yield return null;
            }
            stateMachine.ChangeMovementState(motionHandler.valkState);
        }
    }
}
