using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public interface IAcceptRayCast {
        void Accept(IVisitor _visitor);
    }
}
