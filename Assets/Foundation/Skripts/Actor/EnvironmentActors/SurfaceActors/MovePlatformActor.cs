using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class MovePlatformActor : ActorSurface, IAcceptRayCast{

        [SerializeField] private Transform downPoint;
        [SerializeField] private Transform topPoint;
        [SerializeField] private Transform door_1;
        [SerializeField] private Transform door_2;
        //------------
        private bool isStartPoint = true;
        private bool startMove = false;
        private Vector3 downPosition;
        private Vector3 topPosition;

        protected override void Start() {
            base.Start();
            if (isStartPoint) DoorOpen(false);
            downPosition = topPosition = Player.position;
            downPosition.y = downPoint.position.y;
            topPosition.y = topPoint.position.y;
        }

        public void Accept(IVisitor _visitor) {
            _visitor.Visit(this);
            if (!startMove) {
                Task.CreateTask(MovePlatformTop()).Start();
                startMove = true;
            }
        }

        public void Activate() {
            if (Player.position == topPosition) {
                Task.CreateTask(MovePlatformDown()).Start();
                startMove = false;
            }
        }

        private IEnumerator MovePlatformTop() {
            if (isStartPoint) {
                yield return new WaitForSeconds(0.5f);
                DoorOpen(true);
                while(!MoveTop()) {
                    yield return new WaitForFixedUpdate();
                } 
                DoorOpen(false);
            }
        }

        private IEnumerator MovePlatformDown() {
            if (isStartPoint) {
                yield return new WaitForSeconds(0.5f);
                DoorOpen(true);
                while(!MoveDown()) {
                    yield return new WaitForFixedUpdate();
                }
                DoorOpen(false);
            }
        }

        private void DoorOpen(bool _active) {
            if (door_1) door_1.gameObject.SetActive(_active); 
            if (door_2) door_2.gameObject.SetActive(_active);
        }

        private bool MoveTop() {
            Player.position = Vector3.MoveTowards(Player.position, topPosition, 5 * Time.fixedDeltaTime);
            if (Player.position.y != topPosition.y) return false;
            else return true;            
        }

        private bool MoveDown() {
            Player.position = Vector3.MoveTowards(Player.position, downPosition, 5 * Time.fixedDeltaTime);
            if (Player.position.y != downPosition.y) return false;
            else return true;            
        }

    }
}
