using System;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace XRTest.Input
{
    public class InputLogger : ITickable, ILateDisposable
    {
        private readonly XRIDefaultInputActions _inputActions;

        public Vector2 MovementInput { get; private set; }
        public float RotationInput {  get; private set; }

        public Action<Quaternion> LeftThumbstickPressed;
        public Action<bool> LeftTriggerInteracted;
        public Action<bool> RightTriggerInteracted;
        public Action<bool> LeftGripInteracted;
        public Action<bool> RightGripInteracted;

        public InputLogger() 
        {
            _inputActions = new();
            _inputActions.Enable();

            _inputActions.XRILeftHand.ThumbstickPressed.started += OnLeftThumbstickPressed;

            _inputActions.XRILeftHandInteraction.Activate.started += OnLeftTriggerPressed;
            _inputActions.XRILeftHandInteraction.Activate.canceled += OnLeftTriggerReleased;
            _inputActions.XRIRightHandInteraction.Activate.started += OnRightTriggerPressed;
            _inputActions.XRIRightHandInteraction.Activate.canceled += OnRightTriggerReleased;

            _inputActions.XRILeftHandInteraction.Select.started += OnLeftGripPressed;
            _inputActions.XRILeftHandInteraction.Select.canceled += OnLeftGripReleased;
            _inputActions.XRIRightHandInteraction.Select.started += OnRightGripPressed;
            _inputActions.XRIRightHandInteraction.Select.canceled += OnRightGripReleased;
        }

        public void LateDispose()
        {
            _inputActions.Disable();
            _inputActions.Dispose();

            _inputActions.XRILeftHand.ThumbstickPressed.started -= OnLeftThumbstickPressed;

            _inputActions.XRILeftHandInteraction.Activate.started -= OnLeftTriggerPressed;
            _inputActions.XRILeftHandInteraction.Activate.canceled -= OnLeftTriggerReleased;
            _inputActions.XRIRightHandInteraction.Activate.started -= OnRightTriggerPressed;
            _inputActions.XRIRightHandInteraction.Activate.canceled -= OnRightTriggerReleased;

            _inputActions.XRILeftHandInteraction.Select.started -= OnLeftGripPressed;
            _inputActions.XRILeftHandInteraction.Select.canceled -= OnLeftGripReleased;
            _inputActions.XRIRightHandInteraction.Select.started -= OnRightGripPressed;
            _inputActions.XRIRightHandInteraction.Select.canceled -= OnRightGripReleased;
        }

        public void Tick()
        {
            MovementInput = _inputActions.XRILeftHandLocomotion.Move.ReadValue<Vector2>();
            RotationInput = _inputActions.XRIRightHandLocomotion.Move.ReadValue<Vector2>().x;
        }

        private void OnLeftThumbstickPressed(CallbackContext context) 
        {
            LeftThumbstickPressed?.Invoke(_inputActions.XRIHead.Rotation.ReadValue<Quaternion>());
        }

        private void OnLeftTriggerPressed(CallbackContext _) 
        {
            LeftTriggerInteracted?.Invoke(true);
        }

        private void OnLeftTriggerReleased(CallbackContext _) 
        {
            LeftTriggerInteracted?.Invoke(false);
        }

        private void OnRightTriggerPressed(CallbackContext _)
        {
            RightTriggerInteracted?.Invoke(true);
        }

        private void OnRightTriggerReleased(CallbackContext _)
        {
            RightTriggerInteracted?.Invoke(false);
        }

        private void OnLeftGripPressed(CallbackContext _)
        {
            LeftGripInteracted?.Invoke(true);
        }

        private void OnLeftGripReleased(CallbackContext _)
        {
            LeftGripInteracted?.Invoke(false);
        }

        private void OnRightGripPressed(CallbackContext _)
        {
            RightGripInteracted?.Invoke(true);
        }

        private void OnRightGripReleased(CallbackContext _)
        {
            RightGripInteracted?.Invoke(false);
        }
    }
}
