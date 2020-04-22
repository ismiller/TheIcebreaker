using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class ObstacleActor : ActorObstacle, IAcceptRayCast {
        
        protected override void Start() { 
            base.Start();            
        }

        public void Accept(IVisitor _visitor) {
            _visitor.Visit(this);
        }
    }
}
