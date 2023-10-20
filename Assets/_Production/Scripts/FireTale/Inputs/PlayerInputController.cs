using System;
using FT.Tools.Observers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FT.Inputs
{

    [DisallowMultipleComponent]
    public class PlayerInputController : MonoBehaviour, IInputProvider
    {
        private readonly InputData _inputData = new();
        
        public IObservableAction<Action<InputData>> OnInput => _onInput;
        private readonly ObservableAction<Action<InputData>> _onInput = new();

        public void OnMove(InputValue value) => _inputData.moveDirection = value.Get<Vector2>();
        public void OnLook(InputValue value) => _inputData.mousePosition = Mouse.current.position.value;
        public void OnShoot(InputValue value) => _inputData.isShooting = value.isPressed;
        
        private void Update() => 
            _onInput.Action?.Invoke(_inputData);
    }
}