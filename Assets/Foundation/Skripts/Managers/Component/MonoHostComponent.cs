using UnityEngine;

namespace Scaramouche.Game {
    public class MonoHostComponent : MonoBehaviour {

        private UpdateManager manager;
        [SerializeField] [Range(0.0f, 2.0f)] private float FPSUpdateInterval = 1.0F;
        private double lastInterval;
        private int frames = 0;
        private int fps;

        void OnGUI() {
            GUILayout.Label("" + fps.ToString("f2"));
        }

        void FPSUpdate() {
            ++frames;
            float timeNow = Time.realtimeSinceStartup;
            if(timeNow > lastInterval + FPSUpdateInterval) {
                fps = (int)(frames / (timeNow - lastInterval));
                frames = 0;
                lastInterval = timeNow;
            }
        }

        public void Setup(UpdateManager _mng) {
            lastInterval = Time.realtimeSinceStartup;
            this.manager = _mng;
        }

        private void Update() {
            manager.Tick();
            FPSUpdate();
        }

        private void FixedUpdate() {
            manager.TickFixed();
        }
        
        private void LateUpdate() {
            manager.TickLate();
        }    
    }
}
