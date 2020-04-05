using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    public class Starter : MonoBehaviour {
        
        [SerializeField] private List<ManagerBase> managers = new List<ManagerBase>();

        private void Awake() {
            foreach(var manager in managers) {
                if(manager) {
                    Toolbox.Add(manager);
                }
            }
        }
    }
}
