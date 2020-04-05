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

        public bool isCurentState => CurrentState != null;

        public void Initialize(IMovement _startingState) {
            CurrentState = _startingState;
            CurrentState.Enter();
        }

        public void ChangeMovementState(IMovement _newState) {
            CurrentState.Exit();
            CurrentState = _newState;
            CurrentState.Enter();
        }
    }
}
