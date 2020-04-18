using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class MotionStateMachine {

        public IMovement currentState;
        public IMovement CurrentState {
            get { return currentState; }
            private set { currentState = value; }
        }

        public MotionStateMachine(IMovement _startingState) {
            CurrentState = _startingState;
            CurrentState.Enter();
        }

        public static MotionStateMachine Initialize(IMovement _startingState) {
            return new MotionStateMachine(_startingState);            
        }

        public void ChangeMovementState(IMovement _newState) {
            CurrentState.Exit();
            CurrentState = _newState;
            CurrentState.Enter();
        }
    }
}
