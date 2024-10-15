using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRTest.Weapons.Magazines
{
    [RequireComponent(typeof(XRSocketInteractor))]
    internal class MagazineSocket : MonoBehaviour
    {
        private XRSocketInteractor _socketInteractor;
        private Transform _transform;
        private Magazine _attachedMagazine;
        private Type _targetMagazineType;

        internal bool HasMagazine => _attachedMagazine != null;
        internal bool CanFire => _attachedMagazine != null && _attachedMagazine.CurrentBulletCount > 0;
        internal int CurrentBulletCount => _attachedMagazine.CurrentBulletCount;
        internal int MaxBulletCount => _attachedMagazine.MaxBulletCount;

        internal Action<int> MagazineInserted;
        internal Action MagazineRemoved;

        internal void Initialize(Type targetMagazineType) 
        {
            _transform = transform;
            _targetMagazineType = targetMagazineType;
            _socketInteractor = GetComponent<XRSocketInteractor>();
            _socketInteractor.selectEntered.AddListener(OnSelectEntered);
            _socketInteractor.selectExited.AddListener(OnSelectExited);
        }

        internal void ToggleMagazineCollider(bool isEnabled)
        {
            if (_attachedMagazine == null)
                return;

            _attachedMagazine.ToggleCollider(isEnabled);
        }

        internal void ToggleSocketInteractor(bool isEnabled) 
        {
            if (_attachedMagazine != null && isEnabled == false)
                return;

            _socketInteractor.enabled = isEnabled;
        }

        internal void OnDestroy()
        {
            _socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
            _socketInteractor.selectExited.RemoveListener(OnSelectExited);
        }

        internal void RemoveBulletFromMagazine() 
        {
            _attachedMagazine.RemoveTopBullet();
        }

        private void OnSelectEntered(SelectEnterEventArgs args) 
        {
            Transform interactableTransform = args.interactableObject.transform;

            if (interactableTransform.TryGetComponent(out Magazine magazine) == false ||
                magazine.GetType() != _targetMagazineType)
            {
                return;
            }

            _attachedMagazine = magazine;
            interactableTransform.SetParent(_transform);
            MagazineInserted?.Invoke(_attachedMagazine.MaxBulletCount);
        }

        private void OnSelectExited(SelectExitEventArgs args) 
        {
            _attachedMagazine = null;
            args.interactableObject.transform.SetParent(null);
            MagazineRemoved?.Invoke();
        }
    }
}