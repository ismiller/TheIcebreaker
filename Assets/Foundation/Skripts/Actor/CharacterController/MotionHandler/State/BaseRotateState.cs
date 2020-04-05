using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class BaseRotateState {
        
        protected static PlayerMotionHandler motionHandler;
        protected static MotionStateMachine stateMachine;
        protected static CharacterMotor characterMotor;
        protected static PlayerMotionComponent motionComponent;

        protected Transform player;

        protected BaseRotateState(PlayerMotionHandler _motionHandler) {
            motionHandler = _motionHandler;
            characterMotor = _motionHandler.CharacterMotor;
            motionComponent = _motionHandler.MotionComponent;
            stateMachine = _motionHandler.RotateStateMachine;
            player = _motionHandler.CharacterTransform;
        }
    }
}
