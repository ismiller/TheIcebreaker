using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public interface IAcceptVisitorRayCast {
        void AcceptDownCast(IVisitorEnvironment _visitor);
        void AcceptForwardCast(IVisitorEnvironment _visitor);
    }
}
