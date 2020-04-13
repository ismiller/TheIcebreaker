using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public class Toolbox : SinglManager<Toolbox> {

        private Dictionary<Type, object> dataManager = new Dictionary<Type, object>();

        public static void Add(object obj) {
            var add = obj;
            var manager = obj as ManagerBase;
            
            if (manager) {
                add = Instantiate(manager);
            } else return;

            Instance.dataManager.Add(obj.GetType(), add);
            StartAwake(add);
        }

        public static T GetManager<T>() {
            object value;
            Instance.dataManager.TryGetValue(typeof(T), out value);
            return (T)value;
        }

        private static void StartAwake(object obj) {
            if (obj is IAwake) {
                (obj as IAwake).OnAwake();
            } else return;
        }
    }
}
