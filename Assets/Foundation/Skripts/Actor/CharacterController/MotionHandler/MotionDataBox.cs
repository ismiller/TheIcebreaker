using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class MotionDataBox {
        
        private PlayerMotionHandler motionHandler;
        private PlayerMotionComponent motionComponent;
        private PlayerActor playerActor;
        private Transform transform;
        private CharacterMotor characterMotor;
        private MoveStateBox stateBox;
        private MotionStateMachine stateMachine;
        private CharacterController CHController;
        private Animator animator;
        private MediatorController mediator;

        public MotionDataBox(PlayerMotionHandler _motionHandler) {
            motionHandler = _motionHandler;
            playerActor = _motionHandler.GetPlayerActor;
            motionComponent = playerActor.MotionComponent;
            transform = playerActor.Player;
            CHController = playerActor.PlayerCHController;
            animator = playerActor.PlayerAnimator;
            mediator = playerActor.Mediator;
        }

        public PlayerMotionComponent GetMotionComponent { get { return motionComponent; } }
        public PlayerActor GetPlayerActor { get { return playerActor; } }
        public Transform GetTransform { get { return transform; } }
        public CharacterController GetCHController { get { return CHController; } }
        public Animator GetAnimator { get { return animator; } }
        public MediatorController GetMediator { get { return mediator; } }

        public CharacterMotor GetCharacterMotor { 
            get { 
                return characterMotor ?? (characterMotor = new CharacterMotor(motionHandler)); 
            } 
        }

        public MoveStateBox GetStateBox { 
            get { 
                return stateBox ?? (stateBox = new MoveStateBox(motionHandler)); 
            } 
        }
        
        public MotionStateMachine GetStateMachine { 
            get { 
                return stateMachine ?? (stateMachine = new MotionStateMachine(GetStateBox.GetValkState)); 
            }
        }
    }
}
