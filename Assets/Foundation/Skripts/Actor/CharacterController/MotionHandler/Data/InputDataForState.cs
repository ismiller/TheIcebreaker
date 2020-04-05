using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class InputDataForState  {
        
        public bool isGraund;
        public bool isObstacle;
        public bool isSlope;

        public void Clear() {
            isGraund = false;
            isObstacle = false;
            isSlope = false;
        }
    }
}
