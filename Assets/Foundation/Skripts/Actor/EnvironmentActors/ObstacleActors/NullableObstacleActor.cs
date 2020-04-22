using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class NullableObstacleActor : IAcceptRayCast
    {   
        public NullableObstacleActor() { /* */ }

        public void Accept(IVisitor _visitor) {
            _visitor.Visit(this);
        }
    }
}
