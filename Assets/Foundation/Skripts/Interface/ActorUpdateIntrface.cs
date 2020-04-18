using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public interface IUpdateHandler {
        void UpdateHandler();
    }

    public interface IFixedUpdateHandler {
        void FixedUpdateHandler();
    }

    public interface ILateUpdateHandler {
        void LateUpdateHandler();
    }
}
