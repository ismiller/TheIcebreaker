using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class InterplayHandler : BaseMainHandler, IInterplayInterface, IVisitor {
        
        private PlayerActor actor;
        private InterplayComponent rayCastComponent => actor.GetRayCastComponent;
        private ITask downCastTask, forwardCastTask;
        private Vector3 OriginPoint => new Vector3(player.position.x, player.position.y - 0.5f, player.position.z);
        private MediatorController mediator => actor.Mediator;
        private NullableObstacleActor nullable = new NullableObstacleActor();

        public InterplayHandler() : base() { }

        public override void Initialize(ActorCharacter _actor) {
            base.Initialize(_actor);
            actor = (PlayerActor)_actor;
            downCastTask = Task.CreateTask(DownRayCast());
            forwardCastTask = Task.CreateTask(ForwardRayCast());
        }

        public override void StartHandle() {
            downCastTask.Start();
            forwardCastTask.Start();
        }

        public override void StopHandle() {
            downCastTask.Stop();
            forwardCastTask.Stop();
        }

        private IEnumerator DownRayCast() {
            while (true) {
                Ray downRay = new Ray(player.position, Vector3.down);
                if (Physics.Raycast(downRay, out RaycastHit hit, rayCastComponent.DownCast, rayCastComponent.DownLayer)) {
                    CreateAccept(hit);
                }
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        private IEnumerator ForwardRayCast() {
            while (true) {
                Ray forwardRay = new Ray(OriginPoint, player.forward);
                if (Physics.Raycast(forwardRay, out RaycastHit hit, rayCastComponent.ForwardCast, rayCastComponent.ForwardLayer)) {
                    CreateAccept(hit);
                } else { nullable.Accept(this); }
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        private void CreateAccept(RaycastHit _hit) {
            if (_hit.transform.TryGetComponent<IAcceptRayCast>(out IAcceptRayCast _component)) {
                _component.Accept(this);
            }
        }

        
        public void Visit(ObstacleActor _actor) => mediator.SendMessage<M_Obstacle>(new M_Obstacle(_actor));
        public void Visit(SlopeSurfaceActor _actor) => mediator.SendMessage<M_SlopeSurface>(new M_SlopeSurface(_actor));
        public void Visit(DefoltSurfaceActor _actor) => mediator.SendMessage<M_DefoltSurface>(new M_DefoltSurface(_actor));
        public void Visit(MovePlatformActor _actor) => mediator.SendMessage<M_MovePlatform>(new M_MovePlatform(_actor));
        public void Visit(NullableObstacleActor _actor) => mediator.SendMessage<M_Nullable>(new M_Nullable(_actor));

        public void TriggerEnter(Actor _other) => CreateTriggerMesage(TriggerEventType.Enter, _other);
        public void TriggerStay(Actor _other) => CreateTriggerMesage(TriggerEventType.Stay, _other);
        public void TriggerExit(Actor _other) => CreateTriggerMesage(TriggerEventType.Exit, _other);

        public void CreateTriggerMesage(TriggerEventType _type, Actor _other) {
            if (_other is ImpactZoneActor atz) mediator.SendMessage<M_TriggerZone>(new M_TriggerZone(atz, _type));
            if (_other is RemoteControlActor rca) mediator.SendMessage<M_TriggerInteract>(new M_TriggerInteract(rca, _type));
            if (_other is SelectionItemsActor sia) mediator.SendMessage<M_TriggerSelection>(new M_TriggerSelection(sia, _type));
        }
    }
}