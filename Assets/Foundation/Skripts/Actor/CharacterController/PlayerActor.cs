using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class PlayerActor : ActorCharacter {

        [Header("Player Settings")]
        [SerializeField] private CharacterActorParametrs parametrsComponent;
        [Header("Motion Component")]
        [SerializeField] private PlayerMotionComponent motionComponent;
        [Header("Interraction Component")] 
        [SerializeField] private InterplayComponent rayCastComponent;
        //------------
        private MediatorController mediator;
        private CharacterController playerCHController;
        private Animator playerAnimator;
        private bool isFirstStart = true;
        //------------
        public PlayerMotionComponent MotionComponent { get { return motionComponent; } }
        public CharacterActorParametrs ParametrsComponent { get { return parametrsComponent; } }
        public InterplayComponent GetRayCastComponent { get { return rayCastComponent; } }

        public MediatorController Mediator {
            get { return mediator ?? (mediator = new MediatorController()); }
        }

        public List<BaseMainHandler> GetMainHandlers {
            get { return mainHandlers; }
        }

        public CharacterController PlayerCHController { 
            get { return playerCHController ?? (playerCHController = parametrsComponent.AddCharacterController(this)); } 
        }

        public Animator PlayerAnimator { 
            get { return playerAnimator ?? (playerAnimator = parametrsComponent.AddAnimator(this)); } 
        }

        private ITask FindCameraTask {
            get { return Task.CreateTask(SetPlayerInCamera()); }
        }

        protected override void Start() {
            layer = 11;
            FindCameraTask.Start();      
            HandlerInitialize(
                motionComponent.GetHandler(this), 
                rayCastComponent.GetHandler(this));
            UpdateManager.AddTo(this);
            isFirstStart = false;
            base.Start();
        }

        protected override void OnEnable() {
            if (!isFirstStart) {
                base.OnEnable();
                UpdateManager.AddTo(this);
            }
        }

        protected override void OnDisable() {
            base.OnDisable();
            if (!Toolbox.isApplicationQuitting) 
                UpdateManager.RemoveFrom(this);
        }

        private IEnumerator SetPlayerInCamera() {
            CameraActor tempCamera = null;
            while (!tempCamera) {
                tempCamera = GameObject.FindObjectOfType<CameraActor>().GetComponent<CameraActor>();
                yield return new WaitForEndOfFrame();
            }
            tempCamera.SetPlayer(Player);
        }
    }
}
