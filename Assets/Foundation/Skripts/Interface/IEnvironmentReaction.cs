using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public interface IEnvironmentReaction {
        void DefoltSurfaceReaction(DefoltSurfaceActor _actor);
        void ObstacleReaction(ObstacleActor _actor);
        void SlopeSurfaceReaction(SlopeSurfaceActor _actor);
        void SlipperySurfaceReaction(SlipperySurfaceActor _actor);
    }
}
