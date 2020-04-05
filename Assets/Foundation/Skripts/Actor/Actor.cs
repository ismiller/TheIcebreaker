using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scaramouche.Game {
    public abstract class Actor : MonoBehaviour {

        private Transform thisTransform;
        
        public Transform ThisTransform {
            get {
                if (!thisTransform) thisTransform = transform.GetComponent<Transform>();
                return thisTransform; 
            }
            protected set { thisTransform = value; }
        }
    }
}
