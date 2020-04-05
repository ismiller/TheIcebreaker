using UnityEngine;

namespace Scaramouche.Game {
    public class SinglManager<T> : MonoBehaviour where T : MonoBehaviour {  

        private static  T instance;
        
        public static T Instance {
            get {
                if (!instance) {
                    instance = FindObjectOfType<T>();
                    if (!instance) {
                        var singlton = new GameObject("[TOOLBOX]");
                        instance = singlton.AddComponent<T>();
                    }
                }
                return instance;
            }
        }
    }
}
