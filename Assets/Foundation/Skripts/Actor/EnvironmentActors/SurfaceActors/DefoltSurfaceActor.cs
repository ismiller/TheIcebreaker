using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class DefoltSurfaceActor : ActorSurface, IAcceptRayCast {

        protected override void Start() {
            base.Start();
        }

        public void Accept(IVisitor _visitor) {
            _visitor.Visit(this);
        }
    }
}
