using UnityEngine;

namespace Scaramouche.Game {
    [CreateAssetMenu(fileName = "Camera Motion Handler", menuName = "Camera Component/Motion Handler")]
    public class CameraMotionComponent : ControlComponent {

        [SerializeField] private bool isFollow;
        [SerializeField] private bool isRotate;
        [SerializeField] [Range (0.1f, 10.0f)] private float sensitivity;
        [SerializeField] [Range (-30.0f, 30.0f)] private float offset;
        [SerializeField] [Range (5, 30)] private float areaTurnMouse;
        //------------
        public bool IsFollow { get { return isFollow; } }
        public bool ISRotate { get { return isRotate; } }
        public float Sensitivity { get { return sensitivity; } }
        public float Offset { get { return offset; } }
        public float AreaTurnMouse { get { return areaTurnMouse; } }
        //------------
        private CameraMotionHandler mainHandler;
        public CameraMotionHandler MainHandler { get { return mainHandler; } }

        public override void Initialize(Transform _camera) {
            mainHandler = new CameraMotionHandler(_camera.GetComponent<CameraActor>());
        }

        public override BaseMainHandler GetMainHandler() {
            return MainHandler;
        }
    }
}