using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine;

namespace Scaramouche.Game {
    [CreateAssetMenu(fileName = "Character Actor Parametrs", menuName = "Player Component/Character Parametrs")] 
    public class CharacterActorParametrs : ControlComponent<NullableHandler> {

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
        [SerializeField] private AnimatorUpdateMode updateMode;
        [SerializeField] private AnimatorCullingMode cullingMode;

        public Animator AddAnimator(CharacterActor _actor) {
            var tempAnimator = _actor.Player.GetChild(0).transform.gameObject.AddComponent<Animator>();
            tempAnimator.runtimeAnimatorController = animatorController;
            tempAnimator.avatar = playerAvatar;
            tempAnimator.applyRootMotion = aplayRootMotion;
            tempAnimator.updateMode = updateMode;
            tempAnimator.cullingMode = cullingMode;
            return tempAnimator;
        }

        public CharacterController AddCharacterController(CharacterActor _actor) {
            var tempCHController = _actor.Player.gameObject.AddComponent<CharacterController>();
            tempCHController.slopeLimit = slopeLimit;
            tempCHController.stepOffset = stepOffset;
            tempCHController.skinWidth = skinWidth;
            tempCHController.radius = radius;
            tempCHController.height = height;
            return tempCHController;
        }
    }
}
