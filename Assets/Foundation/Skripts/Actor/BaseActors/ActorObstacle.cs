using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class ActorObstacle : Actor {

        protected Collider obstacleCollider;
        public Collider GetObstacleCollider {
            get { return obstacleCollider ?? (obstacleCollider = Player.GetComponent<Collider>()); }
        }

        protected override void Start() {
            layer = 9;
            Player.gameObject.layer = layer;
        }

    }
}
