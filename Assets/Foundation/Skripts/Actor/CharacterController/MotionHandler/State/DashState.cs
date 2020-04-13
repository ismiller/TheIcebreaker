using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class DashState : BaseMovementState, IMovement {
        
        private ITask dashTask;
        private IMovement lastState;

        private bool startDash;
        private bool stopDash;

        public DashState(PlayerMotionHandler _motionHandler, IMovement _lastState) : base(_motionHandler) {
            lastState = _lastState;
        }

        public void Enter() {
            dashTask = Task.CreateTask(DashMove());
            startDash = stopDash = false;
        }

        public DashState Start(Vector3 _direction) {
            direction = _direction;
            return this;
        }

        public void LogicUpdate() {
            if (stopDash) { motionStateMachine.ChangeMovementState(lastState); }
        }

        public void PhisicUpdate() {
            if (direction != Vector2.zero) {
                if (!startDash) {
                    startDash = true;
                    dashTask.Start();
                }
            } else { stopDash = true; }
        }

        public void Exit() {
            dashTask = null;
        }

        private IEnumerator DashMove() {
            while (!stopDash) {
                yield return null;
                if (!characterMotor.MovementDash(direction)) { 
                    stopDash = true; 
                }
            }
        }
    }
}
