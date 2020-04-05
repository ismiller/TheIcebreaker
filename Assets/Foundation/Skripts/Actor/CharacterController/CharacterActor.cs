using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class CharacterActor : Actor, IVisitor {

        [SerializeField] private CharacterActorParametrs parametrsComponent;
        [SerializeField] private PlayerMotionComponent motionComponent;
        public PlayerMotionComponent MotionComponent { get { return motionComponent; } }

        private List<IObstacleReaction> obstacleHandles;
        private List<ISlopeReaction> slopeHandles;
        private List<IGroundDefoltReaction> groundDefoltHandler;

        private CharacterController characterController;

        private CameraActor cameraActor;
        private bool isSliding = false;

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
            obstacleHandles = new List<IObstacleReaction>();
            slopeHandles = new List<ISlopeReaction>();
            groundDefoltHandler = new List<IGroundDefoltReaction>();
        }

        private void AddComponentsInHadles(params ControlComponent[] _components) {
            foreach (var component in _components) {
                AddComponentsInHadles(component);
            }
        }

        private void AddComponentsInHadles(ControlComponent _component) {
            _component.Initialize(ThisTransform);
            BaseMainHandler mainHandlerTemp = _component.GetMainHandler();
            if (mainHandlerTemp is IObstacleReaction) obstacleHandles.Add(mainHandlerTemp as IObstacleReaction);
            if (mainHandlerTemp is ISlopeReaction) slopeHandles.Add(mainHandlerTemp as ISlopeReaction);
            if (mainHandlerTemp is IGroundDefoltReaction) groundDefoltHandler.Add(mainHandlerTemp as IGroundDefoltReaction);
        }

        public void Visit(IAcceptVisitor _acceptVisitor) {
            if (_acceptVisitor is ObstacleActor) VisitHandler((_acceptVisitor as ObstacleActor));
            if (_acceptVisitor is SlopeActor) VisitHandler((_acceptVisitor as SlopeActor));
            if (_acceptVisitor is DefoltSurfaceActor) VisitHandler((_acceptVisitor as DefoltSurfaceActor));
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
        private void VisitHandler(ObstacleActor _obstacle) {
            if (obstacleHandles.Count > 0) {
                foreach (var component in obstacleHandles) {
                    component.JumpObstacle(_obstacle);
                }
            }
        }

        private void VisitHandler(SlopeActor _sliding) {
            if ((!isSliding) && (slopeHandles.Count > 0)) {
                foreach (var component in slopeHandles) {
                    isSliding = component.StartSlidingSlope(_sliding);
                }
            } 
        }

        private void VisitHandler(DefoltSurfaceActor _defoltSurface) {
            if ((isSliding) && (groundDefoltHandler.Count > 0)) {
                foreach (var component in groundDefoltHandler) {
                    isSliding = !component.GroundStateReset(_defoltSurface);
                }
            } 
        }
    #endregion

    }
}
