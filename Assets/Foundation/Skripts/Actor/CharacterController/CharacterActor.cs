using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class CharacterActor : Actor {

        [Header("Player Settings")]
        [SerializeField] private CharacterActorParametrs parametrsComponent;
        [Header("Motion Component")]
        [SerializeField] private PlayerMotionComponent motionComponent;
        [Header("Ray Cast Component")] 
        [SerializeField] private RayCastComponent rayCastComponent;
 
        private MediatorCharacterHandler mediatorHandler;
        //------------
        private CharacterController playerCHController;
        private Animator playerAnimator;
        private bool isFirstStart = true;
        //------------
        private List<ITriggerHandler> triggerHandlers = new List<ITriggerHandler>();
        //------------
        public PlayerMotionComponent MotionComponent { get { return motionComponent; } }
        public CharacterActorParametrs ParametrsComponent { get { return parametrsComponent; } }
        public MediatorCharacterHandler MediatorHandler { get { return mediatorHandler; } }

        public CharacterController PlayerCHController { 
            get { return playerCHController ?? (playerCHController = parametrsComponent.AddCharacterController(this)); } 
        }

        public Animator PlayerAnimator { 
            get { return playerAnimator ?? (playerAnimator = parametrsComponent.AddAnimator(this)); } 
        }

        private ITask FindCameraTask {
            get { return Task.CreateTask(SetPlayerInCamera()); }
        }

        private void Start() {
            FindCameraTask.Start();      
            HandlerInitialize(
                motionComponent.GetHandler(this), 
                rayCastComponent.GetHandler(this));
            AddInTriggerHandles(mainHandlers);
            UpdateManager.AddTo(this);
            isFirstStart = false;
            mediatorHandler = new MediatorCharacterHandler(mainHandlers);
        }

        protected override void OnEnable() {
            if (!isFirstStart) {
                AddInTriggerHandles(mainHandlers);
                base.OnEnable();
                UpdateManager.AddTo(this);
            }
        }

        protected override void OnDisable() {
            triggerHandlers.Clear();
            base.OnDisable();
            if (!Toolbox.isApplicationQuitting) 
                UpdateManager.RemoveFrom(this);
        }

        private void AddInTriggerHandles(List<BaseMainHandler> _handlers) {
            foreach (var handler in _handlers) {
                if (handler is ITriggerHandler) { 
                    triggerHandlers.Add(handler as ITriggerHandler);
                }
            }
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
