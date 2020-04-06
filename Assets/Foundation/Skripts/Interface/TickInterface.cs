using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public interface ITick {
        void Tick();
    }

    public interface ITickFixed {
        void TickFixed();
    }

    public interface ITickLate {
        void TickLate();
    }
}
