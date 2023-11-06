using System;
using System.Collections;
using FT.TD;
using FT.Tools.Observers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FT.Inputs
{

    [DisallowMultipleComponent]
    public class PlayerInputController : MonoBehaviour, IInputProvider
    {
        private readonly InputData _inputData = new();
        private IViewController _viewController;
        
        public IObservableAction<Action<InputData>> OnInput => _onInput;
        private readonly ObservableAction<Action<InputData>> _onInput = new();

        public void OnMove(InputValue value) => _inputData.moveDirection = value.Get<Vector2>();
        public void OnLook(InputValue value) => _inputData.lookRotation = _viewController.HitLocation(value.Get<Vector2>());
        public void OnFire(InputValue value) => _inputData.isShooting = value.isPressed;
        public void OnInventory(InputValue value) => _inputData.isInventory = !_inputData.isInventory;
        public void OnEscape(InputValue value) => StartCoroutine(ToggleValue(() => _inputData.isEscape = true, () => _inputData.isEscape = false));

        private void Awake() => _viewController = GetComponent<IViewController>();

        private void Update() => 
            _onInput.Action?.Invoke(_inputData);
        
        private IEnumerator ToggleValue(Action setTrue, Action setFalse)
        {
            setTrue();
            yield return new WaitForEndOfFrame();
            setFalse();
        }
    }
}