using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {

    public interface IGeterIntrplay {
        bool TryGetInterplayComponent(ref IInterplayInterface _temp); 
    } 

    public interface IInterplayInterface {
        void TriggerEnter(Actor _other);
        void TriggerStay(Actor _other);
        void TriggerExit(Actor _other);
    }
}
