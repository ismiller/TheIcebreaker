using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class PlayerMotionHandler : BaseMainHandler, IUpdateHandler, ILateUpdateHandler {
        
        private PlayerActor actor;
        private CharacterMotor characterMotor;
        private MoveStateBox moveStateBox;
        private MotionStateMachine motionStateMachine;
        private ITask taskSlidingPosition => Task.CreateTask(CalculateSlidingPosition());
        private Vector3 obstaclePoint;
        private SurfaceType surfaceTemp;
        private float distanceStartSliding => actor.MotionComponent.StartSlidingDist;
        private bool isDefoltGround => (surfaceTemp == SurfaceType.Defolt) ? true : false;
        private MediatorController mediator => actor.Mediator;
        //------------
        public PlayerActor GetCharacterActor {
            get { return actor; }
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

        public Vector3[] GetPatchTemp { get; private set; }

        public Vector3 GetObstaclePoint { get { return obstaclePoint; } }

        public bool IsObstacle { get; private set; } = false;
        public bool IsSlope { get; private set; } = false;
        public bool IsBeginSlope { get; private set; } = false;
        public bool IsPlatform { get; private set; } = false;
        public bool IsTrigger { get; private set; } = false;

        private enum SurfaceType { Defolt, Slope, MovePlatform, None }

        public PlayerMotionHandler() : base() { /* */ }

        public override void Initialize(Actor _actor) {
            base.Initialize(_actor);
            actor = (PlayerActor)_actor;
        }

        public override void StartHandle() {
            InputManager.GetKeyInteract += (bool _value) => isinteract = _value;
            mediator.AddSubscribe<M_TriggerInteract>(ReactionOnTrigger);
            mediator.AddSubscribe<M_Nullable>(ReadMesageNulable);
            mediator.AddSubscribe<M_Obstacle>(ReadMesageObstacle);
            mediator.AddSubscribe<M_SlopeSurface>(ReadMesageSlope);
            mediator.AddSubscribe<M_DefoltSurface>(ReadMesageDefolt);
            mediator.AddSubscribe<M_MovePlatform>(ReadMesageMovePlatform); 
        }

        public override void StopHandle() {
            taskSlidingPosition.Stop();
            mediator.RemoveSubscribe<M_TriggerInteract>(ReactionOnTrigger);
            mediator.RemoveSubscribe<M_Nullable>(ReadMesageNulable); 
            mediator.RemoveSubscribe<M_Obstacle>(ReadMesageObstacle);
            mediator.RemoveSubscribe<M_SlopeSurface>(ReadMesageSlope);
            mediator.RemoveSubscribe<M_DefoltSurface>(ReadMesageDefolt);
            mediator.RemoveSubscribe<M_MovePlatform>(ReadMesageMovePlatform);  
        }

        public void UpdateHandler() {
            GetMotionStateMachine.currentState.PhisicUpdate();
        }

        public void LateUpdateHandler() {
            GetMotionStateMachine.currentState.LogicUpdate();
        }

        private IEnumerator CalculateSlidingPosition() {
            float distanceEnd; float distanceStart;
            distanceStart = Vector3.Distance(player.position, GetPatchTemp[0]);
            while ((GetPatchTemp != null) && !IsSlope) {
                distanceEnd = Vector3.Distance(player.position, GetPatchTemp[GetPatchTemp.Length - 1]);
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

        private void ReadMesageNulable(M_Nullable _mesage) {
            IsObstacle = false;
        }

        public void ReadMesageObstacle(M_Obstacle _mesage) {  
            if (_mesage.actor.GetObstacleCollider) {
                obstaclePoint = _mesage.actor.GetObstacleCollider.ClosestPoint(player.position); 
                IsObstacle = true;    
            }
        }

        public void ReadMesageSlope(M_SlopeSurface _mesage) {
            if (isDefoltGround){
                surfaceTemp = SurfaceType.Slope;
                GetPatchTemp = new Vector3[_mesage.actor.GetSlidingPath().Length];
                for (var i = 0; i < GetPatchTemp.Length; i++) {
                    GetPatchTemp[i] = _mesage.actor.GetSlidingPath()[i];
                }
                taskSlidingPosition.Start();
            }
        }

        public void ReadMesageDefolt(M_DefoltSurface _mesage) {
            if (!isDefoltGround) {
                surfaceTemp = SurfaceType.Defolt;
                player.SetParent(null);
                IsSlope = IsPlatform = false;
                GetPatchTemp = null;
            }
        }

        public void ReadMesageMovePlatform(M_MovePlatform _mesage) {
            if (isDefoltGround) {
                player.SetParent(_mesage.actor.Player);
                surfaceTemp = SurfaceType.MovePlatform; 
                IsPlatform = true; 
            }   
        }

        private bool isinteract = false;

        public void ReactionOnTrigger(M_TriggerInteract _mesage) {
            if (_mesage.eventType == TriggerEventType.Enter) {
                IsTrigger = true;
            } else if (_mesage.eventType == TriggerEventType.Exit) {
                IsTrigger = false;
            }   
            if (IsTrigger && isinteract) {
                _mesage.actor.Use();
                isinteract = false;
            } 
        }

    }
}
