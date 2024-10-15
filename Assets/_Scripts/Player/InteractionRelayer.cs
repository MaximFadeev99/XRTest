using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using XRTest.Weapons;

namespace XRTest.MainPlayer
{
    internal class InteractionRelayer
    {
        private readonly XRRayInteractor _leftInteractor;
        private readonly XRRayInteractor _rightInteractor;
        private readonly XRInteractorLineVisual _leftLineVisual;
        private readonly XRInteractorLineVisual _rightLineVisual;

        internal Action<GameObject> ObjectGrabbedWithLeft;
        internal Action<GameObject> ObjectGrabbedWithRight;

        internal Action<GameObject> ObjectDroppedWithLeft;
        internal Action<GameObject> ObjectDroppedWithRight;

        internal InteractionRelayer(XRRayInteractor leftInteractor, XRRayInteractor rightInteractor)
        {
            _leftInteractor = leftInteractor;
            _rightInteractor = rightInteractor;
            _leftLineVisual = _leftInteractor.GetComponent<XRInteractorLineVisual>();
            _rightLineVisual = _rightInteractor.GetComponent<XRInteractorLineVisual>();

            _leftInteractor.hoverEntered.AddListener(OnLeftHoverEntered);
            _leftInteractor.hoverExited.AddListener(OnLeftHoverExited);
            _leftInteractor.selectEntered.AddListener(OnLeftSelectEntered);
            _leftInteractor.selectExited.AddListener(OnLeftSelectExited);

            _rightInteractor.hoverEntered.AddListener(OnRightHoverEntered);
            _rightInteractor.hoverExited.AddListener(OnRightHoverExited);
            _rightInteractor.selectEntered.AddListener(OnRightSelectEntered);
            _rightInteractor.selectExited.AddListener(OnRightSelectExited);
        }

        internal void OnDestroy()
        {
            _leftInteractor.hoverEntered.RemoveListener(OnLeftHoverEntered);
            _leftInteractor.hoverExited.RemoveListener(OnLeftHoverExited);
            _leftInteractor.selectEntered.RemoveListener(OnLeftSelectEntered);

            _rightInteractor.hoverEntered.RemoveListener(OnRightHoverEntered);
            _rightInteractor.hoverExited.RemoveListener(OnRightHoverExited);
            _rightInteractor.selectEntered.RemoveListener(OnRightSelectEntered);
        }

        internal void OnLeftGripInteracted(bool isPressed) 
        {
            _leftLineVisual.enabled = isPressed;
        }

        internal void OnRightGripInteracted(bool isPressed)
        {
            _rightLineVisual.enabled = isPressed;
        }

        private void OnRightHoverEntered(HoverEnterEventArgs args) 
        {
            if (args.interactableObject.transform.gameObject.TryGetComponent(out Slide _))
                _rightInteractor.useForceGrab = false;

            _rightLineVisual.enabled = true;
        }

        private void OnRightHoverExited(HoverExitEventArgs _) 
        {
            _rightInteractor.useForceGrab = true;
            _rightLineVisual.enabled = false;
        }

        private void OnLeftHoverEntered(HoverEnterEventArgs args) 
        {
            if (args.interactableObject.transform.gameObject.TryGetComponent(out Slide _))
                _leftInteractor.useForceGrab = false;

            _leftLineVisual.enabled = true;
        }

        private void OnLeftHoverExited(HoverExitEventArgs _) 
        {
            _leftInteractor.useForceGrab = true;
            _leftLineVisual.enabled = false;
        }

        private void OnLeftSelectEntered(SelectEnterEventArgs enterEventArgs) 
        {
            ObjectGrabbedWithLeft?.Invoke(enterEventArgs.interactableObject.transform.gameObject);
        }

        private void OnRightSelectEntered(SelectEnterEventArgs enterEventArgs) 
        {
            ObjectGrabbedWithRight?.Invoke(enterEventArgs.interactableObject.transform.gameObject);
        }

        private void OnLeftSelectExited(SelectExitEventArgs exitEventArgs)
        {
            ObjectDroppedWithLeft?.Invoke(exitEventArgs.interactableObject.transform.gameObject);
        }

        private void OnRightSelectExited(SelectExitEventArgs exitEventArgs)
        {
            ObjectDroppedWithRight?.Invoke(exitEventArgs.interactableObject.transform.gameObject);
        }
    }
}