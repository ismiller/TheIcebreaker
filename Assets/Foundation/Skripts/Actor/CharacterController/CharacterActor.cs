using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class CharacterActor : Actor, IVisitorEnvironment {

        [SerializeField] private CharacterActorParametrs parametrsComponent;
        [SerializeField] private PlayerMotionComponent motionComponent;
        public PlayerMotionComponent MotionComponent { get { return motionComponent; } }

        private List<IEnvironmentReaction> environmentReactions;

        private CharacterController characterController;

        private CameraActor cameraActor;

        private void Start() {
            ThisTransform = transform.GetComponent<Transform>();
            cameraActor = GameObject.FindObjectOfType<CameraActor>().GetComponent<CameraActor>();
            AddCharacterController();
            ListStartInitialize();
            AddComponentsInHadles(motionComponent);
            //Task.CreateTask(SphereOverlapCast()).Start();
            Task.CreateTask(GraundRayCast()).Start();
            Task.CreateTask(SetPlayerInCamera()).Start();
            UpdateManager.AddTo(this);
        }

        private void AddCharacterController() {
            characterController = ThisTransform.gameObject.AddComponent<CharacterController>();
            characterController.slopeLimit = parametrsComponent.SlopeLimit;
            characterController.stepOffset = parametrsComponent.StepOffset;
            characterController.skinWidth = parametrsComponent.SkinWidth;
            characterController.radius = parametrsComponent.Radius;
            characterController.height = parametrsComponent.Height;
        }

        private void ListStartInitialize() {
            environmentReactions = new List<IEnvironmentReaction>();
        }

        private void AddComponentsInHadles(params ControlComponent[] _components) {
            foreach (var component in _components) {
                AddComponentsInHadles(component);
            }
        }

        private void AddComponentsInHadles(ControlComponent _component) {
            _component.Initialize(ThisTransform);
            BaseMainHandler mainHandlerTemp = _component.GetMainHandler();
            if (mainHandlerTemp is IEnvironmentReaction) environmentReactions.Add(mainHandlerTemp as IEnvironmentReaction);
        }

        private IEnumerator SetPlayerInCamera() {
            yield return new WaitForEndOfFrame();
            cameraActor.SetPlayer(ThisTransform);
        }

    #region  Trigger_And_RayCast
        private IEnumerator SphereOverlapCast() {
            while (true) {
                RaycastHit[] hitAll;
                hitAll = Physics.SphereCastAll(ThisTransform.position, 5.0f, Vector3.forward, 0.0f);
                if (hitAll.Length > 0) {
                    foreach (var hit in hitAll) {
                        yield return new WaitForEndOfFrame();
                        if (hit.transform.TryGetComponent<IAcceptVisitor>(out IAcceptVisitor _actor)) {
                            _actor.AcceptRayCast(this);
                            yield return new WaitForEndOfFrame();
                        }
                    }
                }
                yield return new WaitForSeconds(.3f);
            }
        }

        private IEnumerator GraundRayCast() {
            while (true) {
                Ray downRay = new Ray(ThisTransform.position, Vector3.down);
                if (Physics.Raycast(downRay, out RaycastHit hit,1.5f)) {
                    yield return new WaitForEndOfFrame();
                    if (hit.transform.TryGetComponent<IAcceptVisitor>(out IAcceptVisitor _actor)) {
                        _actor.AcceptRayCast(this);
                        yield return new WaitForEndOfFrame();
                    }
                }
                yield return new WaitForSeconds(.3f);
            }
        }

        private void OnTriggerEnter(Collider other) {
            if(other.transform.TryGetComponent<IAcceptVisitor>(out IAcceptVisitor _actor)) {
                _actor.AcceptOnTrigger(this);
            }
        }

        private void OnTriggerExit(Collider other) {
            if(other.transform.TryGetComponent<IAcceptVisitor>(out IAcceptVisitor _actor)) {
                _actor.AcceptOnTrigger(this);
            }
        }

        private void OnTriggerStay(Collider other) {
            if(other.transform.TryGetComponent<IAcceptVisitor>(out IAcceptVisitor _actor)) {
                _actor.AcceptOnTrigger(this);
            }
        }
    #endregion

    #region VisitHandler
        public void Visit(ObstacleActor _actor) {
            if (environmentReactions.Count > 0) {
                foreach (var component in environmentReactions) {
                    component.ObstacleReaction(_actor);
                }
            }
        }

        public void Visit(SlopeSurfaceActor _actor) {
            if (environmentReactions.Count > 0) {
                foreach (var component in environmentReactions) {
                    component.SlopeSurfaceReaction(_actor);
                }
            } 
        }

        public void Visit(DefoltSurfaceActor _actor) {
            if (environmentReactions.Count > 0) {
                foreach (var component in environmentReactions) {
                    component.DefoltSurfaceReaction(_actor);
                }
            } 
        }

        public void Visit(SlipperySurfaceActor _actor) {
            if (environmentReactions.Count > 0) {
                foreach (var component in environmentReactions) {
                    component.SlipperySurfaceReaction(_actor);
                }
            } 
        }
    #endregion

    }
}
