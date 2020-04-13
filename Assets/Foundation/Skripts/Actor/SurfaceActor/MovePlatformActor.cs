using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class MovePlatformActor : Actor, IAcceptVisitorRayCast{

        [SerializeField] private Transform downPoint;
        [SerializeField] private Transform topPoint;
        [SerializeField] private Transform door_1;
        [SerializeField] private Transform door_2;
        //------------
        private bool isStartPoint = true;
        private bool startMove = false;
        private Vector3 downPosition;
        private Vector3 topPosition;
        //------------
        private delegate bool Move();
        Move move;

        private void Start() {
            ThisTransform = GetComponent<Transform>();
            if (isStartPoint) DoorOpen(false);
            downPosition = topPosition = ThisTransform.position;
            downPosition.y = downPoint.position.y;
            topPosition.y = topPoint.position.y;
            move = MoveAxisY;
        }

        public void AcceptDownCast(IVisitorEnvironment _visitor) {
            _visitor.Visit(this);
        }

        public void AcceptForwardCast(IVisitorEnvironment _visitor) {
            //пустота
        }

        public void StartMove() {
            if (!startMove) {
                Task.CreateTask(MovePlatform()).Start();
                startMove = true;
            }
        }

        private IEnumerator MovePlatform() {
            if (isStartPoint) {
                yield return new WaitForSeconds(0.5f);
                DoorOpen(true);
                while(!move()) {
                    yield return new WaitForFixedUpdate();
                } 
                DoorOpen(false);
            }
        }

        private void DoorOpen(bool _active) {
            if (door_1) door_1.gameObject.SetActive(_active); 
            if (door_2) door_2.gameObject.SetActive(_active);
        }

        private bool MoveAxisY() {
            ThisTransform.position = Vector3.MoveTowards(ThisTransform.position, topPosition, 3 * Time.fixedDeltaTime);
            if (ThisTransform.position.y != topPosition.y) return false;
            else return true;            
        }
    }
}
