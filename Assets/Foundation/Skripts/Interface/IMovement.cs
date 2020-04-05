using UnityEngine;

namespace Scaramouche.Game {
    public interface IMovement {

        void Enter();
        void LogicUpdate();
        void PhisicUpdate();
        void Exit();
        
    }
}
