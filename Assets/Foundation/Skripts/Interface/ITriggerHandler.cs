using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public interface ITriggerHandler {
        void TriggerEnter(Collider _other);
        void TriggerStay(Collider _other);
        void TriggerExit(Collider _other);
    }
}
