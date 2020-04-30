using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class PlayerMotionHandler : BaseMainHandler, IUpdateHandler, ILateUpdateHandler {
        
        private enum SurfaceType { Defolt, Slope, MovePlatform, None }
        
        private PlayerActor actor;
        private MotionDataBox dataBox;
        private SurfaceType surfaceTemp;

        private ITask taskSlidingPosition => Task.CreateTask(CalculateSlidingPosition());
        private bool isDefoltGround => (surfaceTemp == SurfaceType.Defolt) ? true : false;
        //------------
        public PlayerActor GetPlayerActor { get { return actor; } }
        public MotionDataBox GetDataBox { get { return dataBox; } }

        public Vector3[] GetPatchTemp { get; private set; }
        public Vector3 GetObstaclePoint { get; private set; }
        public bool IsObstacle { get; private set; } = false;
        public bool IsSlope { get; private set; } = false;
        public bool IsBeginSlope { get; private set; } = false;
        public bool IsPlatform { get; private set; } = false;
        public bool IsTrigger { get; private set; } = false;

        public PlayerMotionHandler() : base() { /* */ }

        public override void Initialize(ActorCharacter _actor) {
            base.Initialize(_actor);
            actor = (PlayerActor)_actor;
            dataBox = new MotionDataBox(this);
        }

        public override void StartHandle() {
            InputManager.GetKeyInteract += (bool _value) => isinteract = _value;
            dataBox.GetMediator.AddSubscribe<M_TriggerInteract>(ReactionOnTrigger);
            dataBox.GetMediator.AddSubscribe<M_Nullable>(ReadMesageNulable);
            dataBox.GetMediator.AddSubscribe<M_Obstacle>(ReadMesageObstacle);
            dataBox.GetMediator.AddSubscribe<M_SlopeSurface>(ReadMesageSlope);
            dataBox.GetMediator.AddSubscribe<M_DefoltSurface>(ReadMesageDefolt);
            dataBox.GetMediator.AddSubscribe<M_MovePlatform>(ReadMesageMovePlatform); 
        }

        public override void StopHandle() {
            taskSlidingPosition.Stop();
            dataBox.GetMediator.RemoveSubscribe<M_TriggerInteract>(ReactionOnTrigger);
            dataBox.GetMediator.RemoveSubscribe<M_Nullable>(ReadMesageNulable); 
            dataBox.GetMediator.RemoveSubscribe<M_Obstacle>(ReadMesageObstacle);
            dataBox.GetMediator.RemoveSubscribe<M_SlopeSurface>(ReadMesageSlope);
            dataBox.GetMediator.RemoveSubscribe<M_DefoltSurface>(ReadMesageDefolt);
            dataBox.GetMediator.RemoveSubscribe<M_MovePlatform>(ReadMesageMovePlatform);  
        }

        public void UpdateHandler() {
            dataBox.GetStateMachine.currentState.PhisicUpdate();
        }

        public void LateUpdateHandler() {
            dataBox.GetStateMachine.currentState.LogicUpdate();
        }

        private IEnumerator CalculateSlidingPosition() {
            float distanceEnd; float distanceStart;
            distanceStart = Vector3.Distance(player.position, GetPatchTemp[0]);
            while ((GetPatchTemp != null) && !IsSlope) {
                distanceEnd = Vector3.Distance(player.position, GetPatchTemp[GetPatchTemp.Length - 1]);
                if (distanceStart < distanceEnd) {
                    IsSlope = IsBeginSlope = true;
                } else if (distanceEnd < distanceStart) {
                    if(distanceEnd > dataBox.GetMotionComponent.StartSlidingDist) {
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
                GetObstaclePoint = _mesage.actor.GetObstacleCollider.ClosestPoint(player.position); 
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
