using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public abstract class Actor : MonoBehaviour, ITick, ITickFixed, ITickLate {

        private Transform player;

        protected List<IUpdateActor> updateActors = new List<IUpdateActor>();
        protected List<IFixedUpdateActor> fixedUpdateActors = new List<IFixedUpdateActor>();
        protected List<ILateUpdateActor> lateUpdateActors = new List<ILateUpdateActor>();
        
        public Transform Player {
            get {
                if (!player) {
                    player = transform.GetComponent<Transform>();
                }
                return player; 
            }
        }

        protected void AddTo(object updateObj) {
            if (updateObj is IUpdateActor) {
                this.updateActors.Add(updateObj as IUpdateActor);
            }
            
            if (updateObj is IFixedUpdateActor) {
                this.fixedUpdateActors.Add(updateObj as IFixedUpdateActor);
            }

            if (updateObj is ILateUpdateActor) {
                this.lateUpdateActors.Add(updateObj as ILateUpdateActor);
            }
        }

        protected void RemoveFrom(object updateObj) {
            if (updateObj is IUpdateActor) {
                this.updateActors.Remove(updateObj as IUpdateActor);
            }
            
            if (updateObj is IFixedUpdateActor) {
                this.fixedUpdateActors.Remove(updateObj as IFixedUpdateActor);
            }

            if (updateObj is ILateUpdateActor) {
                this.lateUpdateActors.Remove(updateObj as ILateUpdateActor);
            }
        }  

        public void ClearUpdateLists() {
            updateActors.Clear();
            lateUpdateActors.Clear();
            fixedUpdateActors.Clear();
        }

        public void Tick() { 
            for (int i = 0; i < updateActors.Count; i++) {
                updateActors[i].UpdateActor();
            }
        }

        public void TickFixed() { 
            for (int i = 0; i < fixedUpdateActors.Count; i++) {
                fixedUpdateActors[i].FixedUpdateActor();
            }
        }

        public void TickLate() { 
            for (int i = 0; i < lateUpdateActors.Count; i++) {
                lateUpdateActors[i].LateUpdateActor();
            }
        }       
    }
}
