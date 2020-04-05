using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {    
    [CreateAssetMenu(fileName = "Test Motion Handler", menuName = "Player Component/Test Motion Handler")]
    public class PlayerMotionComponent : ControlComponent {
        
        [Header("Rotate Parametrs")]
        [SerializeField] [Range(0, 20)] private float rotateSpeed;
        [Header("Gravity Parametrs")]
        [SerializeField] [Range(0, 20)] private float gravitySpeed;
        [Header("Movement Parametrs")]
        [SerializeField] [Range(0, 20)] private float forvardSpeed;
        [SerializeField] [Range(0, 20)] private float backSpeed;
        [SerializeField] [Range(0, 20)] private float sideSpeed;
        [SerializeField] [Range(0, 10)] private int forvardAngleApp;
        [SerializeField] [Range(0, 10)] private int backAngleApp;
        [SerializeField] [Range(0, 10)] private int sideAngleApp;
        [Header("Dash Parametrs")]
        [SerializeField] [Range(0, 30)] private float dashSpeed;
        [SerializeField] [Range(0, 20)] private float dashTime;
        [SerializeField] [Range(0, 1f)] private float dashStep; 
        [SerializeField] [Range(0, 20)] private float returnTimeDash;
        [Header("Sliding Parametrs")] 
        [SerializeField] [Range(0, 30)] private float speedIfBeginSlope;
        [SerializeField] [Range(0, 30)] private float speedIfEndSlope; 
        [SerializeField] [Range(0, 10)] private float distanceStartSliding;
        //-------------
        public float RotateSpeed { get { return rotateSpeed; } }
        //-------------
        public float GravitySpeed { get { return gravitySpeed; } }
        //-------------
        public float ForvardSpeed { get { return forvardSpeed; } }
        public float BackSpeed { get { return backSpeed; } }
        public float SideSpeed { get { return sideSpeed; } }
        public float ForvardAngleApp { get { return forvardAngleApp * 0.1f; } }
        public float BackAngleApp { get { return backAngleApp * 0.1f; } }
        public float SideAngleApp { get { return sideAngleApp * 0.1f; } }
        //-------------
        public float DashSpeed { get { return dashSpeed; } }
        public float DashTime { get { return dashTime; } }
        public float DashStep { get { return dashStep; } }
        public float ReturnTimeDash { get { return returnTimeDash; } }
        //-------------
        public float SpeedIfBeginSlope { get { return speedIfBeginSlope; } }
        public float SpeedIfEndSlope { get { return speedIfEndSlope; } }
        public float StartSlidingDist { get { return distanceStartSliding; } }
        //-------------

        private PlayerMotionHandler mainHandler;
        public PlayerMotionHandler MainHandler { get { return mainHandler; } }

        public override void Initialize(Transform _player) {
            mainHandler = new PlayerMotionHandler(_player.GetComponent<CharacterActor>());
        }

        public override BaseMainHandler GetMainHandler() {
            return MainHandler;
        }
    }
}
