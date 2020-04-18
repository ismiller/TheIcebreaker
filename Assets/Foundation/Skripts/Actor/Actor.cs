using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public abstract class Actor : MonoBehaviour, ITick, ITickFixed, ITickLate {

        private Transform player;

        protected List<IUpdateHandler> updateActors = new List<IUpdateHandler>();
        protected List<IFixedUpdateHandler> fixedUpdateActors = new List<IFixedUpdateHandler>();
        protected List<ILateUpdateHandler> lateUpdateActors = new List<ILateUpdateHandler>();
        protected List<BaseMainHandler> mainHandlers = new List<BaseMainHandler>();

        public Transform Player {
            get { return player ?? (player = transform.GetComponent<Transform>()); }
        }
        
        protected void AddTo(object updateObj) {
            if (updateObj is IUpdateHandler) {
                this.updateActors.Add(updateObj as IUpdateHandler);
            }
            
            if (updateObj is IFixedUpdateHandler) {
                this.fixedUpdateActors.Add(updateObj as IFixedUpdateHandler);
            }

            if (updateObj is ILateUpdateHandler) {
                this.lateUpdateActors.Add(updateObj as ILateUpdateHandler);
            }
        }

        protected void RemoveFrom(object updateObj) {
            if (updateObj is IUpdateHandler) {
                this.updateActors.Remove(updateObj as IUpdateHandler);
            }
            
            if (updateObj is IFixedUpdateHandler) {
                this.fixedUpdateActors.Remove(updateObj as IFixedUpdateHandler);
            }

            if (updateObj is ILateUpdateHandler) {
                this.lateUpdateActors.Remove(updateObj as ILateUpdateHandler);
            }
        } 

        protected virtual void OnEnable() {
            StartHandler();
        }

        protected virtual void OnDisable() {
            StopHandler();
        }

        protected void StartHandler() {
            if (mainHandlers.Count > 0) {
                foreach (var handler in mainHandlers) {
                    handler.StartHandle();
                }
            }
        }

        protected void StopHandler() {
            if (mainHandlers.Count > 0) {
                foreach (var handler in mainHandlers) {
                    handler.StopHandle();
                }
            }
        }

        protected void HandlerInitialize(params BaseMainHandler[] _handlers) {
            foreach (var handler in _handlers) {
                HandlerInitialize(handler);
            }
        }

        protected void HandlerInitialize(BaseMainHandler handler) {
            this.AddTo(handler);
            handler.StartHandle();
            mainHandlers.Add(handler);
        } 

        protected void ClearUpdateLists() {
            updateActors.Clear();
            lateUpdateActors.Clear();
            fixedUpdateActors.Clear();
        }

        public void Tick() { 
            for (int i = 0; i < updateActors.Count; i++) {
                updateActors[i].UpdateHandler();
            }
        }

        public void TickFixed() { 
            for (int i = 0; i < fixedUpdateActors.Count; i++) {
                fixedUpdateActors[i].FixedUpdateHandler();
            }
        }

        public void TickLate() { 
            for (int i = 0; i < lateUpdateActors.Count; i++) {
                lateUpdateActors[i].LateUpdateHandler();
            }
        }       
    }
}
