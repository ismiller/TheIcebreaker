using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace Scaramouche.Game {
    [CreateAssetMenu(fileName = "InputManager", menuName = "Managers/InputManagers")]
    public class InputManager : ManagerBase, IAwake, ITick {
        
        [SerializeField] private bool _findMousePosition = true;

        private InputContoller _inputController;
        private Vector3 _lastMousePoint;
        private Camera _camera;

        private InputContoller.PlayerActions _actionsList;

        public delegate void GetInputValue<T>(T _value);

        public static event GetInputValue<Vector3> GetMousePoint;
        public static event GetInputValue<Vector2> GetVectorMove;
        public static event GetInputValue<bool> GetKeyShoot;
        public static event GetInputValue<bool> GetKeyAim;
        public static event GetInputValue<bool> GetKeyBendDown;
        public static event GetInputValue<bool> GetKeyDash;
        public static event GetInputValue<bool> GetKeyInteract;
        public static event GetInputValue<float> GetKeyRotate;        

        public void OnAwake () {
            _inputController = new InputContoller();
            _inputController.Enable();
            _actionsList = _inputController.Player;
            _camera = Camera.main;
            Subscrible();
            UpdateManager.AddTo(this);
        }

        private void Subscrible() {
            _actionsList.Movement.performed += ctx => ConvertOnePress(GetVectorMove, ctx.ReadValue<Vector2>());
            _actionsList.Aim.performed += ctx => ConvertOnePress(GetKeyAim, ctx.ReadValue<float>());
            _actionsList.Shoot.performed += ctx => ConvertOnePress(GetKeyShoot, ctx.ReadValue<float>());
            _actionsList.BendDown.performed += ctx => ConvertOnePress(GetKeyBendDown, ctx.ReadValue<float>());
            _actionsList.Dash.performed += ctx => ConvertOnePress(GetKeyDash, ctx.ReadValue<float>());
            _actionsList.Interact.performed += ctx => ConvertOnePress(GetKeyInteract, ctx.ReadValue<float>());
            _actionsList.CameraLeftRotate.performed += ctx => ConvertOnePress(GetKeyRotate, ctx.ReadValue<float>());
        }

        private void ConvertOnePress(GetInputValue<bool> _action, float _value) {
            if(_action != null) {
                _action(_value > 0 ? true : false);
            }
        }

        private void ConvertOnePress(GetInputValue<Vector2> _action, Vector2 _value) {
            if(_action != null) {
                _action(_value);
            }
        }

        private void ConvertOnePress(GetInputValue<float> _action, float _value) {
            if(_action != null) {
                _action(_value);
            }
        }

        public void Tick() {
            if(GetMousePoint != null) {
                FindMousePosition();
            }
        }

        private void FindMousePosition() {
            if(_findMousePosition && _camera) {
                if(GetMousePoint != null) {
                    Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
                    if(Physics.Raycast(ray, out RaycastHit hit, 100.0f)) {
                        _lastMousePoint = hit.point;
                    }
                    GetMousePoint(_lastMousePoint);
                }
            }
        }
    }
}
