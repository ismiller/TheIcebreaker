using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class DefoltRotateState : BaseRotateState, IMovement {

        private Vector3 direction;

        public DefoltRotateState(PlayerMotionHandler _motionHandler) : base(_motionHandler) {

        }

        public void Enter() { 
            InputManager.GetMousePoint += (Vector3 _value) => direction = _value;
        }

        public void LogicUpdate() {
            if(motionHandler.IsSlope) { stateMachine.ChangeMovementState(motionHandler.rotateSlidingSlopeState); }
        }

        public void PhisicUpdate() {  
            if (!motionHandler.IsSlope) {
                if(direction != Vector3.zero) {
                    characterMotor.RotateDirection(direction - player.position);
                }
            }
        }

        public void Exit() {          
            InputManager.GetMousePoint -= (Vector3 _value) => direction = _value;
        }

    }
}
