using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public abstract class ControlComponent : ScriptableObject {

        public abstract void Initialize(Transform _player);
        public abstract BaseMainHandler GetMainHandler();
    }
}
