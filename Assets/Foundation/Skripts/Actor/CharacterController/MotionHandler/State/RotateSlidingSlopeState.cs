using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class RotateSlidingSlopeState : BaseRotateState, IMovement {
        
        private Vector3 direction;
        private int nextPoint;
        private float currentSpeed;

        public RotateSlidingSlopeState(PlayerMotionHandler _motionHandler) : base(_motionHandler) {

        }

        public void Enter() { 
            direction = characterMotor.SearchStartPointInArray(out nextPoint);
        }

        public void LogicUpdate() {
            if (!motionHandler.IsSlope) { Task.CreateTask(EndSliding()).Start(); }
        }

        private Vector3 targetDirection;

        public void PhisicUpdate() {  
            if(Vector3.Distance(player.position, direction) < 1) {
                if ((++nextPoint) < motionHandler.PatchTemp.Length) {
                    direction = motionHandler.PatchTemp[nextPoint];
                    targetDirection = direction - player.position;
                } else { targetDirection = targetDirection.normalized; }
            }
            if(targetDirection != Vector3.zero) {
                characterMotor.RotateDirection(targetDirection);
            }
        }

        public void Exit() {   

        }

        private IEnumerator EndSliding() {
            while (currentSpeed > 0) {
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, Time.deltaTime);
                yield return null;
            }
            stateMachine.ChangeMovementState(motionHandler.defoltRotateState);
        }
    }
}
