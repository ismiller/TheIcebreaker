using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class ValkState : BaseMovementState, IMovement {

        private DashState dashState;
        private int slidingPoint;
        private float currentSpeed;

        public ValkState(PlayerMotionHandler _motionHandler) : base(_motionHandler) {
            dashState = new DashState(motionHandler, this);
            InputManager.GetVectorMove += (_value) => direction = _value;
            dashPossible = true;
        }

        public void Enter() {
            currentSpeed = motionComponent.ForvardSpeed;
            InputManager.GetKeyDash += SetInDash;
        }

        public void LogicUpdate() {
            if (motionHandler.IsSlope) { stateMachine.ChangeMovementState(motionHandler.slidingSlopeState); }
            if (motionHandler.IsSlippery) { stateMachine.ChangeMovementState(motionHandler.slippingState); }
        }

        public void PhisicUpdate() {
            characterMotor.MovementDirectional(direction);
        }

        public void Exit() {
            InputManager.GetKeyDash -= SetInDash;
        }

        private void SetInDash(bool _value) {
            if (direction != Vector2.zero) {
                if (dashPossible) {
                    stateMachine.ChangeMovementState(dashState.Start(direction));
                    Task.CreateTask(DashReturn()).Start();
                }
            }
        }

    }
}