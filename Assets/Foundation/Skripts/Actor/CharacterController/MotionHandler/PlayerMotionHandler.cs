using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class PlayerMotionHandler : BaseMainHandler, IVisitorEnvironment, IUpdateActor, ILateUpdateActor {
        
        private PlayerMotionComponent motionComponent;
        //------------
        private CharacterController characterController;
        private MotionStateMachine movementStateMachine;
        private CharacterMotor characterMotor;
        private Animator playerAnimator;
        //------------
        public readonly SlidingSlopeState slidingSlopeState;
        public readonly JumpObstacleState jumpObstacleState;
        public readonly ValkState valkState;
        //------------
        private Vector3[] patchTemp;
        private Vector3 obstaclePoint;
        private bool isObstacle;
        private bool isSlope;
        private bool isBeginSlope;
        private bool isSlippery;
        private bool isPlatform;
        //------------
        public PlayerMotionComponent MotionComponent { get { return motionComponent; } }
        //------------
        public MotionStateMachine MovementStateMachine { get { return movementStateMachine; } }
        public CharacterMotor CharacterMotor { get { return characterMotor; } }
        public Animator PlayerAnimator { get { return playerAnimator; } }
        //------------
        public Vector3[] PatchTemp { get { return patchTemp; } }
        public Vector3 ObstaclePoint { get { return obstaclePoint; } }
        
        public bool IsGraund { get { return characterController.isGrounded; } }
        public bool IsObstacle { get { return isObstacle; } }
        public bool IsSlope { get { return isSlope; } }
        public bool IsBeginSlope { get { return isBeginSlope; } }
        public bool IsSlippery { get { return isSlippery; } }
        public bool IsPlatform { get { return isPlatform; } }
        //-------------
        private bool isDefoltGround => (surfaceTemp == SurfaceType.Defolt) ? true : false;
        //-------------
        private SurfaceType surfaceTemp = SurfaceType.Defolt;
        private enum SurfaceType {
            Defolt, Slope, MovePlatform, None
        }

        public PlayerMotionHandler(CharacterActor _characterActor) : base (_characterActor) {
            motionComponent = _characterActor.MotionComponent;
            characterController = _characterActor.PlayerCharacterController;
            movementStateMachine = new MotionStateMachine();
            characterMotor = new CharacterMotor(this);
            playerAnimator = _characterActor.PlayerAnimator;
            //------------
            slidingSlopeState = new SlidingSlopeState(this);
            jumpObstacleState = new JumpObstacleState(this);
            valkState = new ValkState(this);
            //------------
            movementStateMachine.Initialize(valkState);
            //------------
            Task.CreateTask(GraundRayCast()).Start();
            Task.CreateTask(ForvardRayCast()).Start();
        }

        public void UpdateActor() {
            movementStateMachine.currentState.PhisicUpdate();
        }

        public void LateUpdateActor() {
            movementStateMachine.currentState.LogicUpdate();
        }

        private IEnumerator GraundRayCast() {
            while (true) {
                if (motionComponent.GroundRayCast) {
                    Ray downRay = new Ray(CharacterTransform.position, Vector3.down);
                    if (Physics.Raycast(downRay, out RaycastHit hit, 1.3f)) {
                        yield return new WaitForEndOfFrame();
                        if (hit.transform.TryGetComponent<IAcceptVisitorRayCast>(out IAcceptVisitorRayCast _actor)) {
                            _actor.AcceptDownCast(this);
                            yield return new WaitForEndOfFrame();
                        }
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        private IEnumerator ForvardRayCast() {
            while (true) {
                Vector3 startPosition = new Vector3(CharacterTransform.position.x, CharacterTransform.position.y - 0.5f, CharacterTransform.position.z);
                Ray forwardRay = new Ray(startPosition, CharacterTransform.forward);
                if (Physics.Raycast(forwardRay, out RaycastHit hit, 1.3f)) {
                    yield return new WaitForEndOfFrame();
                    if (hit.transform.TryGetComponent<IAcceptVisitorRayCast>(out IAcceptVisitorRayCast _actor)) {
                        obstaclePoint = hit.collider.ClosestPoint(CharacterTransform.position);
                        yield return new WaitForEndOfFrame();
                        if (Vector3.Distance(CharacterTransform.position, obstaclePoint) < 0.8f) {
                            _actor.AcceptForwardCast(this);
                        }
                    }
                } else { isObstacle = false; }
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator CalculateSlidingPosition() {
            int endElement  = PatchTemp.Length - 1;
            float distanceEnd; float distanceStart;
            distanceStart = Vector3.Distance(CharacterTransform.position, PatchTemp[0]);
            yield return new WaitForEndOfFrame();
            while ((patchTemp != null) && !isSlope) {
                distanceEnd = Vector3.Distance(CharacterTransform.position, PatchTemp[endElement]);
                yield return new WaitForEndOfFrame();
                if (distanceStart < distanceEnd) {
                    isSlope = true; 
                    isBeginSlope = true;
                } else if (distanceEnd < distanceStart) {
                    if(distanceEnd > motionComponent.StartSlidingDist) {
                        isSlope = true;
                        isBeginSlope = false;
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public void Visit(ObstacleActor _actor) {   
            isObstacle = true;
        }

        public void Visit(SlopeSurfaceActor _actor) {
            if (isDefoltGround){
                surfaceTemp = SurfaceType.Slope;
                patchTemp = new Vector3[_actor.GetSlidingPath().Length];
                for (var i = 0; i < patchTemp.Length; i++) {
                    patchTemp[i] = _actor.GetSlidingPath()[i];
                }
                Task.CreateTask(CalculateSlidingPosition()).Start();
            }
        }

        public void Visit(DefoltSurfaceActor _actor) {
            if (!isDefoltGround) {
                CharacterTransform.SetParent(null);
                surfaceTemp = SurfaceType.Defolt;
                isSlope = isSlippery = isPlatform = false;
                patchTemp = null;
            }
        }

        public void Visit(MovePlatformActor _actor) {
            if (isDefoltGround) {
                CharacterTransform.SetParent(_actor.Player);
                surfaceTemp = SurfaceType.MovePlatform; 
                isPlatform = true; 
                _actor.StartMove();
            }   
        }
    }
}
