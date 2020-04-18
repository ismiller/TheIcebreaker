using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class MoveStateBox {

        private SlidingSlopeState slidingSlopeState;
        private JumpObstacleState jumpObstacleState;
        private DashState dashState;
        private ValkState valkState;

        public SlidingSlopeState GetSlidingSlopeState {
            get { return slidingSlopeState; }
        }

        public JumpObstacleState GetJumpObstacleState {
            get { return jumpObstacleState; }
        }

        public DashState GetDashState {
            get { return dashState; }
        }

        public ValkState GetValkState {
            get { return valkState; }
        }

        public MoveStateBox(PlayerMotionHandler _motionHandler) {
            slidingSlopeState = new SlidingSlopeState(_motionHandler);
            jumpObstacleState = new JumpObstacleState(_motionHandler);
            dashState = new DashState(_motionHandler);
            valkState = new ValkState(_motionHandler);
        }
    }
}
