using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class PlayerMotionHandler : BaseMainHandler, IVisitorEnvironment, IUpdateHandler, ILateUpdateHandler, IMessageRecipients {
        
        private CharacterActor characterActor;
        private CharacterMotor characterMotor;
        private MoveStateBox moveStateBox;
        private MotionStateMachine motionStateMachine;
        private ITask taskGraundCast, taskForvardCast; 
        private ITask taskSlidingPosition => Task.CreateTask(CalculateSlidingPosition());
        private Vector3[] patchTemp;
        private Vector3 obstaclePoint;
        private SurfaceType surfaceTemp;
        private float distanceStartSliding => characterActor.MotionComponent.StartSlidingDist;
        private bool isDefoltGround => (surfaceTemp == SurfaceType.Defolt) ? true : false;

        //------------
        public CharacterActor GetCharacterActor {
            get { return characterActor; }
        }

        public MoveStateBox GetMoveStateBox {
            get { return moveStateBox ?? (moveStateBox = new MoveStateBox(this)); }
        }

        public MotionStateMachine GetMotionStateMachine { 
            get { return motionStateMachine ?? (motionStateMachine = MotionStateMachine.Initialize(GetMoveStateBox.GetValkState)); } 
        }

        public CharacterMotor GetCharacterMotor { 
            get { return characterMotor ?? (characterMotor = new CharacterMotor(this)); } 
        }

        public Vector3[] GetPatchTemp { get { return patchTemp; } }

        public Vector3 GetObstaclePoint { get { return obstaclePoint; } }

        public bool IsObstacle { get; private set; }
        public bool IsSlope { get; private set; }
        public bool IsBeginSlope { get; private set; }
        public bool IsPlatform { get; private set; }

        private enum SurfaceType {
            Defolt, Slope, MovePlatform, None
        }

        public PlayerMotionHandler() : base() {

        }

        public override void Initialize(Actor _actor) {
            base.Initialize(_actor);
            characterActor = (CharacterActor)_actor;
        }

        public override void StartHandle() {

        }

        public override void StopHandle() {
            taskSlidingPosition.Stop();
        }

        public void Notify<T>(T _actor) {
            if (_actor is IAcceptVisitorRayCast) CallAccept(_actor as IAcceptVisitorRayCast);
        }

        private void CallAccept(IAcceptVisitorRayCast _actor) {
            _actor.AcceptDownCast(this);
            _actor.AcceptForwardCast(this);
        }

        public void UpdateHandler() {
            GetMotionStateMachine.currentState.PhisicUpdate();
        }

        public void LateUpdateHandler() {
            GetMotionStateMachine.currentState.LogicUpdate();
        }

        private IEnumerator CalculateSlidingPosition() {
            float distanceEnd; float distanceStart;
            distanceStart = Vector3.Distance(CharacterTransform.position, patchTemp[0]);
            while ((patchTemp != null) && !IsSlope) {
                distanceEnd = Vector3.Distance(CharacterTransform.position, patchTemp[patchTemp.Length - 1]);
                if (distanceStart < distanceEnd) {
                    IsSlope = IsBeginSlope = true;
                } else if (distanceEnd < distanceStart) {
                    if(distanceEnd > distanceStartSliding) {
                        IsSlope = true;
                        IsBeginSlope = false;
                    }
                }
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        public void Visit(ObstacleActor _actor) {   
            IsObstacle = true;
        }

        public void Visit(SlopeSurfaceActor _actor) {
            if (isDefoltGround){
                surfaceTemp = SurfaceType.Slope;
                patchTemp = new Vector3[_actor.GetSlidingPath().Length];
                for (var i = 0; i < patchTemp.Length; i++) {
                    patchTemp[i] = _actor.GetSlidingPath()[i];
                }
                taskSlidingPosition.Start();
            }
        }

        public void Visit(DefoltSurfaceActor _actor) {
            if (!isDefoltGround) {
                CharacterTransform.SetParent(null);
                surfaceTemp = SurfaceType.Defolt;
                IsSlope = IsPlatform = false;
                patchTemp = null;
            }
        }

        public void Visit(MovePlatformActor _actor) {
            if (isDefoltGround) {
                CharacterTransform.SetParent(_actor.Player);
                surfaceTemp = SurfaceType.MovePlatform; 
                IsPlatform = true; 
                _actor.StartMove();
            }   
        }
        
    }
}
