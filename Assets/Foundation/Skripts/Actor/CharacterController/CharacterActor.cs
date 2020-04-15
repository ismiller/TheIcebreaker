using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class CharacterActor : Actor {

        [SerializeField] private CharacterActorParametrs parametrsComponent;
        [SerializeField] private PlayerMotionComponent motionComponent;
        //------------
        private CharacterController playerCharacterController;
        private Animator playerAnimator;
        private bool isFirstStart = true;
        private bool isToolboxActive => Toolbox.isApplicationQuitting;
        //------------
        private List<ITriggerHandler> triggerHandlers = new List<ITriggerHandler>();
        //------------
        public PlayerMotionComponent MotionComponent { get { return motionComponent; } }
        public CharacterActorParametrs ParametrsComponent { get { return parametrsComponent; } }

        public CharacterController PlayerCharacterController { 
            get {
                if (!playerCharacterController) { 
                    playerCharacterController = parametrsComponent.AddCharacterController(this); 
                }
                return playerCharacterController; 
            } 
        }

        public Animator PlayerAnimator { 
            get { 
                if (!playerAnimator) { 
                    playerAnimator = parametrsComponent.AddAnimator(this); 
                }
                return playerAnimator; 
            } 
        }

        private ITask FindCameraTask {
            get { return Task.CreateTask(SetPlayerInCamera()); }
        }

        private void Start() {
            FindCameraTask.Start();      
            ComponentInitialize(motionComponent);
            AddInTriggerHandles(motionComponent);
            UpdateManager.AddTo(this);
            isFirstStart = false;
        }

        private void OnEnable() {
            if (!isFirstStart) {
                ComponentInitialize(motionComponent);
                AddInTriggerHandles(motionComponent);
                UpdateManager.AddTo(this);  
            }
        }

        private void OnDisable() {
            triggerHandlers.Clear();
            ClearUpdateLists();
            if(!isToolboxActive) UpdateManager.RemoveFrom(this);
        }

        private void ComponentInitialize(ControlComponent[] _components) {
            foreach (var component in _components) {
                ComponentInitialize(component);
            }
        }

        private void ComponentInitialize(ControlComponent _component) {
            _component.Initialize(Player);
            this.AddTo(_component.GetMainHandler());
        }

        private void AddInTriggerHandles(ControlComponent[] _components) {
            foreach(var component in _components) {
                AddInTriggerHandles(component);
            }
        }

        private void AddInTriggerHandles(ControlComponent _component) {
            if (_component.GetMainHandler() is ITriggerHandler) { 
                triggerHandlers.Add(_component.GetMainHandler() as ITriggerHandler);
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
