using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class ActorCharacter : Actor, IGeterIntrplay, ITick, ITickFixed, ITickLate {

        protected List<IUpdateHandler> updateActors = new List<IUpdateHandler>();
        protected List<IFixedUpdateHandler> fixedUpdateActors = new List<IFixedUpdateHandler>();
        protected List<ILateUpdateHandler> lateUpdateActors = new List<ILateUpdateHandler>();
        protected List<BaseMainHandler> mainHandlers = new List<BaseMainHandler>();

        protected IInterplayInterface interplayComonent;
        protected bool isInterplay => interplayComonent != null; 
        private bool isFirstStart = true;

        protected override void Start() {
            UpdateManager.AddTo(this);
            isFirstStart = false;
            base.Start();
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

        protected virtual void OnEnable()  {
            StartHandler();
            if (!isFirstStart) {
                UpdateManager.AddTo(this);
            }
        }
        protected virtual void OnDisable() {
            StopHandler();
            if (!Toolbox.isApplicationQuitting) 
                UpdateManager.RemoveFrom(this);
        }

        public bool TryGetInterplayComponent(ref IInterplayInterface _temp) {
            if (isInterplay) {
                _temp = interplayComonent;
                return true;
            } else return false;
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
            if (!isInterplay) {
                if (handler is IInterplayInterface) {
                    interplayComonent = (handler as IInterplayInterface);
                }
            }
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
