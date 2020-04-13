using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class BaseMovementState {

        protected static PlayerMotionComponent motionComponent;
        protected static PlayerMotionHandler motionHandler;
        protected static CharacterMotor characterMotor;
        protected static Animator playerAnimator;
        protected static Transform player;

        protected MotionStateMachine motionStateMachine;
        protected Vector2 direction;
        protected Vector3 rotateDirection;
        protected bool dashPossible;
        protected bool isDash;

        protected BaseMovementState(PlayerMotionHandler _motionHandler) {
            motionHandler = _motionHandler;
            characterMotor = _motionHandler.CharacterMotor;
            motionComponent = _motionHandler.MotionComponent;
            motionStateMachine = _motionHandler.MovementStateMachine;
            player = _motionHandler.CharacterTransform;
            playerAnimator = _motionHandler.PlayerAnimator;
        }

        protected IEnumerator DashReturn() {
            dashPossible = false;
            yield return new WaitForSeconds(motionComponent.ReturnTimeDash);
            dashPossible = true;
        }

    }
}
