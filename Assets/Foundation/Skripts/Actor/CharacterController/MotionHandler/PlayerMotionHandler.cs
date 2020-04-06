using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class PlayerMotionHandler : BaseMainHandler, IEnvironmentReaction, ITick, ITickLate {
        
        private PlayerMotionComponent motionComponent;
        //------------
        private CharacterController characterController;
        private MotionStateMachine rotateStateMachine;
        private MotionStateMachine movementStateMachine;
        private CharacterMotor characterMotor;
        //------------
        public readonly DefoltRotateState defoltRotateState;
        public readonly RotateSlidingSlopeState rotateSlidingSlopeState;
        public readonly SlidingSlopeState slidingSlopeState;
        public readonly SlippingState slippingState;
        public readonly ValkState valkState;
        //------------
        public PlayerMotionComponent MotionComponent { get { return motionComponent; } }
        //------------
        public MotionStateMachine RotateStateMachine { get { return rotateStateMachine; } }
        public MotionStateMachine MovementStateMachine { get { return movementStateMachine; } }
        public CharacterMotor CharacterMotor { get { return characterMotor; } }
        //------------
        public Vector3[] PatchTemp { get { return patchTemp; } }
        public bool IsGraund { get { return characterController.isGrounded; } }
        public bool IsObstacle { get { return isObstacle; } }
        public bool IsSlope { get { return isSlope; } }
        public bool IsBeginSlope { get { return isBeginSlope; } }
        public bool IsSlippery { get { return isSlippery; } }
        //------------
        private Vector3[] patchTemp;
        private bool isObstacle;
        private bool isSlope;
        private bool isBeginSlope;
        private bool isSlippery;
        //-------------
        private bool isDefoltGround => (surfaceTemp == SurfaceType.Defolt) ? true : false;
        //-------------
        private SurfaceType surfaceTemp = SurfaceType.Defolt;
        private enum SurfaceType {
            Defolt, Slope, Slippery, None
        }

        public PlayerMotionHandler(CharacterActor _characterActor) : base (_characterActor) {
            motionComponent = _characterActor.MotionComponent;
            characterController = _characterActor.GetComponent<CharacterController>();
            rotateStateMachine = new MotionStateMachine();
            movementStateMachine = new MotionStateMachine();
            characterMotor = new CharacterMotor(this);
            //------------
            defoltRotateState = new DefoltRotateState(this);
            rotateSlidingSlopeState = new RotateSlidingSlopeState(this);
            slidingSlopeState = new SlidingSlopeState(this);
            slippingState = new SlippingState(this);
            valkState = new ValkState(this);
            //------------
            rotateStateMachine.Initialize(defoltRotateState);
            movementStateMachine.Initialize(valkState);
        }

        public void Tick() {
            rotateStateMachine.currentState.PhisicUpdate();
            movementStateMachine.currentState.PhisicUpdate();
        }

        public void TickLate() {
            rotateStateMachine.currentState.LogicUpdate();
            movementStateMachine.currentState.LogicUpdate();
        }

        public void DefoltSurfaceReaction(DefoltSurfaceActor _actor) {
            if (!isDefoltGround) {
                surfaceTemp = SurfaceType.Defolt;
                isSlope = isSlippery = false;
                patchTemp = null;
            }
        }

        public void ObstacleReaction(ObstacleActor _obstacle) {

        }

        public void SlopeSurfaceReaction(SlopeSurfaceActor _actor) {
            if (isDefoltGround){
                surfaceTemp = SurfaceType.Slope;
                patchTemp = new Vector3[_actor.GetSlidingPath().Length];
                for (var i = 0; i < patchTemp.Length; i++) {
                    patchTemp[i] = _actor.GetSlidingPath()[i];
                }
                Task.CreateTask(CalculateSlidingPosition()).Start();
            }
        }

        public void SlipperySurfaceReaction(SlipperySurfaceActor _actor) {
            if (isDefoltGround) {
                surfaceTemp = SurfaceType.Slippery;
                isSlippery = true;
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
    }
}
