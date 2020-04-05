using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scaramouche.Game {
    [CreateAssetMenu(fileName = "Character Actor Parametrs", menuName = "Player Component/Character Parametrs")] 
    public class CharacterActorParametrs : ScriptableObject {

        [Header("Character Controller")]
        [SerializeField] private float slopeLimit;
        [SerializeField] private float stepOffset;
        [SerializeField] private float skinWidth;
        [SerializeField] private float radius;
        [SerializeField] private float height;

        public float SlopeLimit { get { return slopeLimit; } }
        public float StepOffset { get { return stepOffset; } }
        public float SkinWidth { get { return skinWidth; } }
        public float Radius { get { return radius; } }
        public float Height { get { return height; } }
    }
}
