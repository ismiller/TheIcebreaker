using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
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
        [Header("Animator")]
        [SerializeField] private RuntimeAnimatorController animatorController;
        [SerializeField] private Avatar playerAvatar;
        [SerializeField] private bool aplayRootMotion;
        [SerializeField] private AnimatorCullingMode cullingMode;

        public float SlopeLimit { get { return slopeLimit; } }
        public float StepOffset { get { return stepOffset; } }
        public float SkinWidth { get { return skinWidth; } }
        public float Radius { get { return radius; } }
        public float Height { get { return height; } }
        //------------
        public RuntimeAnimatorController AnimatorController { get { return animatorController; } }
        public Avatar PlayerAvatar { get { return playerAvatar; } }
        public bool AplayRootMotion { get { return aplayRootMotion; } }
        public AnimatorCullingMode CullingMode { get { return cullingMode; } }

        public Animator AddAnimator(CharacterActor _actor) {
            var tempAnimator = _actor.Player.GetChild(0).transform.gameObject.AddComponent<Animator>();
            tempAnimator.runtimeAnimatorController = _actor.ParametrsComponent.AnimatorController;
            tempAnimator.avatar = _actor.ParametrsComponent.PlayerAvatar;
            tempAnimator.applyRootMotion = _actor.ParametrsComponent.AplayRootMotion;
            tempAnimator.cullingMode = _actor.ParametrsComponent.CullingMode;
            return tempAnimator;
        }

        public CharacterController AddCharacterController(CharacterActor _actor) {
            var tempCHController = _actor.Player.gameObject.AddComponent<CharacterController>();
            tempCHController.slopeLimit = _actor.ParametrsComponent.SlopeLimit;
            tempCHController.stepOffset = _actor.ParametrsComponent.StepOffset;
            tempCHController.skinWidth = _actor.ParametrsComponent.SkinWidth;
            tempCHController.radius = _actor.ParametrsComponent.Radius;
            tempCHController.height = _actor.ParametrsComponent.Height;
            return tempCHController;
        }
    }
}
