using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class BaseMovementState {

        protected static PlayerMotionComponent motionComponent;
        protected static PlayerMotionHandler motionHandler;
        protected static CharacterMotor characterMotor;
        protected static Transform player;

        protected MotionStateMachine stateMachine;
        protected Vector2 direction;
        protected Vector3 moveDirection;
        protected bool dashPossible;
        protected bool isDash;

        protected BaseMovementState(PlayerMotionHandler _motionHandler) {
            motionHandler = _motionHandler;
            characterMotor = _motionHandler.CharacterMotor;
            motionComponent = _motionHandler.MotionComponent;
            stateMachine = _motionHandler.MovementStateMachine;
            player = _motionHandler.CharacterTransform;
        }

        protected IEnumerator DashReturn() {
            dashPossible = false;
            yield return new WaitForSeconds(motionComponent.ReturnTimeDash);
            dashPossible = true;
        }

    }
}
