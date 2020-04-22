using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public abstract class Actor : MonoBehaviour {
                
        protected LayerMask layer;
        private Transform player;

        public Transform Player {
            get { return player ?? (player = transform.GetComponent<Transform>()); }
        }

        protected virtual void Start() { 
            Player.gameObject.layer = layer;
        }        
    }
}
