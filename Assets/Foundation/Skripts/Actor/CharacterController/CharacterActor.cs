using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class CharacterActor : Actor {

        [SerializeField] private CharacterActorParametrs parametrsComponent;
        [SerializeField] private PlayerMotionComponent motionComponent;

        public PlayerMotionComponent MotionComponent { get { return motionComponent; } }

        private CharacterController playerCharacterController;
        private Animator playerAnimator;
        private List<ITriggerHandler> triggerHandlers;
        private CameraActor cameraActor;
        //-----------
        public CharacterController PlayerCharacterController { get {return playerCharacterController; } }
        public Animator PlayerAnimator { get { return playerAnimator; } }

        private void Start() {
            ThisTransform = transform.GetComponent<Transform>();
            cameraActor = GameObject.FindObjectOfType<CameraActor>().GetComponent<CameraActor>();
            triggerHandlers = new List<ITriggerHandler>();
            AddCharacterController();
            AddAnimator();
            Task.CreateTask(SetPlayerInCamera()).Start();
            ComponentInitialize(motionComponent);
            AddHandlerInList(motionComponent);
            UpdateManager.AddTo(this);
        }

        private void ComponentInitialize(ControlComponent[] _components) {
            foreach (var component in _components) {
                ComponentInitialize(component);
            }
        }

        private void ComponentInitialize(ControlComponent _component) {
            _component.Initialize(ThisTransform);
            this.AddTo(_component.GetMainHandler());
        }

        private void AddHandlerInList(ControlComponent[] _components) {
            foreach(var component in _components) {
                AddHandlerInList(component);
            }
        }

        private void AddHandlerInList(ControlComponent _component) {
            BaseMainHandler handler = _component.GetMainHandler();
            if (handler is ITriggerHandler) { 
                triggerHandlers.Add(handler as ITriggerHandler);
            }
        }

        private void AddCharacterController() {
            playerCharacterController = ThisTransform.gameObject.AddComponent<CharacterController>();
            playerCharacterController.slopeLimit = parametrsComponent.SlopeLimit;
            playerCharacterController.stepOffset = parametrsComponent.StepOffset;
            playerCharacterController.skinWidth = parametrsComponent.SkinWidth;
            playerCharacterController.radius = parametrsComponent.Radius;
            playerCharacterController.height = parametrsComponent.Height;
        }

        private void AddAnimator() {
            playerAnimator = ThisTransform.GetChild(0).transform.gameObject.AddComponent<Animator>();
            playerAnimator.runtimeAnimatorController = parametrsComponent.AnimatorController;
            playerAnimator.avatar = parametrsComponent.PlayerAvatar;
            playerAnimator.applyRootMotion = parametrsComponent.AplayRootMotion;
            playerAnimator.cullingMode = parametrsComponent.CullingMode;
        }

        private IEnumerator SetPlayerInCamera() {
            yield return new WaitForEndOfFrame();
            cameraActor.SetPlayer(ThisTransform);
        }
    }
}
