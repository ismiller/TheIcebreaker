using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public interface IUpdateActor {
        void UpdateActor();
    }

    public interface IFixedUpdateActor {
        void FixedUpdateActor();
    }

    public interface ILateUpdateActor {
        void LateUpdateActor();
    }
}
