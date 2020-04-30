using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class BaseMovementState {

        protected PlayerMotionHandler motionHandler;
        protected MotionDataBox dataBox;

        protected Vector2 direction;
        protected Vector3 rotateDirection;
        protected bool dashPossible;
        protected bool isDash;

        protected BaseMovementState(PlayerMotionHandler _motionHandler) {
            motionHandler = _motionHandler;
            dataBox = _motionHandler.GetDataBox;
        }
    }
}
