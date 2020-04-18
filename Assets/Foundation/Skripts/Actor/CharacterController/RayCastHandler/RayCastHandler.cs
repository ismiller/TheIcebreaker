using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class RayCastHandler : BaseMainHandler, IMessageRecipients {
        
        private CharacterActor characterActor;
        private MediatorCharacterHandler mediatorHandler => characterActor.MediatorHandler;
        private ITask taskGraundCast;
        private ITask taskForvardCast;
        private IAcceptVisitorRayCast currentGround;

        public RayCastHandler() : base() { }

        public override void Initialize(Actor _actor) {
            base.Initialize(_actor);
            characterActor = (CharacterActor)_actor;
            taskGraundCast = Task.CreateTask(GraundRayCast());
            taskForvardCast = Task.CreateTask(ForvardRayCast());
        }

        public override void StartHandle() {
            taskGraundCast.Start();
        }

        public override void StopHandle() {
            taskGraundCast.Stop();
        }

        public void Notify<T>(T _mesage) {
            
        }

        private IEnumerator GraundRayCast() {
            while (true) {
                Ray downRay = new Ray(CharacterTransform.position, Vector3.down);
                if (Physics.Raycast(downRay, out RaycastHit hit, 1.3f)) {
                    yield return new WaitForSeconds(Time.deltaTime);
                    if (hit.transform.TryGetComponent<IAcceptVisitorRayCast>(out IAcceptVisitorRayCast _actor)) {
                        if (currentGround != _actor) {
                            mediatorHandler.Send<IAcceptVisitorRayCast>(_actor);
                            currentGround = _actor;
                            Debug.Log("Попали!");
                        }
                        yield return new WaitForSeconds(Time.deltaTime);
                    }
                }
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        private IEnumerator ForvardRayCast() {
            while (true) {
                Vector3 startPosition = new Vector3(CharacterTransform.position.x, CharacterTransform.position.y - 0.5f, CharacterTransform.position.z);
                Ray forwardRay = new Ray(startPosition, CharacterTransform.forward);
                if (Physics.Raycast(forwardRay, out RaycastHit hit, 1.3f)) {
                    if (hit.transform.TryGetComponent<IAcceptVisitorRayCast>(out IAcceptVisitorRayCast _actor)) {
                        Vector3 obstaclePoint = hit.collider.ClosestPoint(CharacterTransform.position);
                        yield return new WaitForSeconds(Time.deltaTime);
                        if (Vector3.Distance(CharacterTransform.position, obstaclePoint) < 0.8f) {
                            //_actor.AcceptForwardCast(this);
                        }
                    }
                } //else { IsObstacle = false; }
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}