using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    [System.Serializable] 
    public class CameraParametr {
        [SerializeField] [Range (0.1f, 10.0f)] private float sensitivity;
        [SerializeField] [Range (-30.0f, 30.0f)] private float offset;
        [SerializeField] [Range (5, 30)] private float areaTurnMouse;
        
        public float Sensitivity { get { return sensitivity; } }
        public float Offset { get { return offset; } }
        public float AreaTurnMouse { get { return areaTurnMouse; } }
    }
}
