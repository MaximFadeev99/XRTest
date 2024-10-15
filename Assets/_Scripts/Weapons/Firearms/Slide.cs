using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRTest.Weapons
{
    public class Slide : MonoBehaviour
    {
        private Transform _transform;
        private Collider _collider;
        private Animator _animator;
        private Transform _parentTransform;
        private XRGrabInteractable _grabInteractable;
        private CancellationTokenSource _cts;
        private float _pulledLocalZ;

        internal bool IsPulled { get; private set; } = false;

        internal Action Pulled;

        internal void Initialize(Transform parentTransform, Animator animator, float pulledOffset)
        {
            _transform = transform;
            _parentTransform = parentTransform;
            _animator = animator;
            _pulledLocalZ = _transform.localPosition.z - pulledOffset;
            _collider = GetComponent<Collider>();
            _grabInteractable = GetComponent<XRGrabInteractable>();
            _grabInteractable.selectEntered.AddListener(OnSelectEntered);
            _grabInteractable.selectExited.AddListener(OnSelectExited);
        }

        internal void ResetPulledProperty() 
        {
            IsPulled = false;
        }

        internal void ToggleCollider(bool isEnabled) 
        {
            _collider.enabled = isEnabled;
        }

        private void OnDestroy()
        {
            _grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            _grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }

        private void OnSelectEntered(SelectEnterEventArgs __) 
        {
            _animator.enabled = false;
            _transform.SetParent(_parentTransform);
            _cts = new();
            _ = TrackZPosition(_cts.Token);
        }

        private void OnSelectExited(SelectExitEventArgs _) 
        {
            if(_cts.Token.CanBeCanceled)
                _cts.Cancel();

            _animator.enabled = true;
        }

        private async UniTaskVoid TrackZPosition(CancellationToken cancellationToken) 
        {
            while (_transform.localPosition.z > _pulledLocalZ && cancellationToken.IsCancellationRequested == false) 
                await UniTask.Yield();

            if (cancellationToken.IsCancellationRequested)
                return;

            IsPulled = true;
            Pulled?.Invoke();
        }
    }
}
