using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class SlippingState : BaseMovementState, IMovement {

        public SlippingState(PlayerMotionHandler _motionHandler) : base(_motionHandler) {

        }

        public void Enter() {
            Debug.Log("Вошли в состояние скольжения!");
        }

        public void LogicUpdate() {

        }

        public void PhisicUpdate() {

        }

        public void Exit() {
            
        }

    }
}
