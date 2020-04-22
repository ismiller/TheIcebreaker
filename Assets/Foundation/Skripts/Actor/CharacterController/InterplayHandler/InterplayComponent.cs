using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    [CreateAssetMenu(fileName = "Ray Cast Component", menuName = "Player Component/Ray Cast Component")]
    public class InterplayComponent : ControlComponent<InterplayHandler> {
        
        [SerializeField] [Range(0, 5)] private float forwardCast;
        [SerializeField] private LayerMask forwardLayer;
        [SerializeField] [Range(0, 5)] private float downCast;
        [SerializeField] private LayerMask downLayer;
        //------------
        public float ForwardCast { get { return forwardCast; } }
        public LayerMask ForwardLayer { get { return forwardLayer; } }
        public float DownCast { get { return downCast; } }
        public LayerMask DownLayer { get { return downLayer; } }
    }
}
