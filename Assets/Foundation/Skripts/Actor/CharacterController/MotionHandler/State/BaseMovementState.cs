using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class BaseMovementState {

        protected PlayerMotionHandler motionHandler;
        protected Transform player => motionHandler.GetCharacterActor.Player;
        protected CharacterMotor characterMotor => motionHandler.GetCharacterMotor;
        protected Animator playerAnimator => motionHandler.GetCharacterActor.PlayerAnimator;
        protected MotionStateMachine motionStateMachine => motionHandler.GetMotionStateMachine;
        protected CharacterController chController => motionHandler.GetCharacterActor.PlayerCHController;
        protected PlayerMotionComponent motionComponent => motionHandler.GetCharacterActor.MotionComponent;

        protected Vector2 direction;
        protected Vector3 rotateDirection;
        protected bool dashPossible;
        protected bool isDash;

        protected BaseMovementState(PlayerMotionHandler _motionHandler) {
            motionHandler = _motionHandler;
        }
    }
}
