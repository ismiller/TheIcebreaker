using UnityEngine;

namespace Scaramouche.Game {
    public class SinglManager<T> : MonoBehaviour where T : MonoBehaviour {  

        private static  T instance;
        private static System.Object locked = new System.Object();
        public static bool isApplicationQuitting = false;

        public static T Instance {
            get {
                lock (locked) {
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

        public virtual void OnDestroy() {
            isApplicationQuitting = true;
        }
    }
}
