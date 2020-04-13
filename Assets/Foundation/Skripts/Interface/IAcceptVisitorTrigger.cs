using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public interface IAcceptVisitorTrigger {
        void AcceptOnTriggerEnter(IVisitorEnvironment _visitor);
        void AcceptOnTriggerStay(IVisitorEnvironment _visitor);
        void AcceptOnTriggerExit(IVisitorEnvironment _visitor);
    }
}
