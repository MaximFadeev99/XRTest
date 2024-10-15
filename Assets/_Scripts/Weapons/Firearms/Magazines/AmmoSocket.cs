using Cysharp.Threading.Tasks;
using ModestTree;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRTest.Weapons.Magazines
{
    internal class AmmoSocket : XRSocketInteractor
    {
        private int _maxBulletCount;
        private Type _targetBulletType;
        private Transform _transform;
        private GameObject _topBullet;
        private IXRSelectInteractable[] _loadedBullets;

        protected override int socketSnappingLimit => _maxBulletCount;
        internal int CurrentBulletCount { get; private set; } = 0;

        internal Action BulletLoaded;

        internal void Initialize(int maxBulletCount, Type targetBulletType) 
        {
            _transform = transform;
            _maxBulletCount = maxBulletCount;
            _targetBulletType = targetBulletType;
            _loadedBullets = new IXRSelectInteractable[_maxBulletCount];
            TryFindPreloadedBullets();
        }

        internal void RemoveTopBullet() 
        {
            if (_topBullet != null) 
            {
                UnregisterBullet(_topBullet.GetComponent<IXRSelectInteractable>());
                Destroy(_topBullet);
            }

            CurrentBulletCount--;
            IXRSelectInteractable nextBullet = _loadedBullets.FirstOrDefault(bullet => bullet != null);

            if (nextBullet != null)
            {
                _topBullet = nextBullet.transform.gameObject;
                _topBullet.SetActive(true);
            }

            socketActive = true;
        }

        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            if (_loadedBullets.Contains(interactable))
                return true;

            bool isInsertingRightBullet = interactable.colliders[0].gameObject.TryGetComponent(_targetBulletType, out _);
            showInteractableHoverMeshes = CurrentBulletCount < _maxBulletCount && isInsertingRightBullet;

            return base.CanSelect(interactable);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            LoadBullet(args.interactableObject);
        }

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);

            if (CurrentBulletCount >= _maxBulletCount)
            {
                socketActive = false;
                return;
            }
            else 
            {
                socketActive = true;
            }

            if (_topBullet != null)
                _topBullet.SetActive(false);
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);

            if (_topBullet != null)
                _topBullet.SetActive(true);
        }

        private void RegisterBullet(IXRSelectInteractable newBullet) 
        {
            for (int i = 0;  i < _loadedBullets.Length; i++) 
            {
                if (_loadedBullets[i] == null)
                {
                    _loadedBullets[i] = newBullet;
                    return;
                }
            }
        }

        private void UnregisterBullet(IXRSelectInteractable bulletToUnregister) 
        {
            int targetIndex = _loadedBullets.IndexOf(bulletToUnregister);

            if (targetIndex != -1)
                _loadedBullets[targetIndex] = null;
        }

        private void TryFindPreloadedBullets() 
        {
            IXRSelectInteractable[] childObjects = _transform.GetComponentsInChildren<IXRSelectInteractable>();

            for (int i = 0; i < childObjects.Length; i++) 
                LoadBullet(childObjects[i], false);
        }

        private async void LoadBullet(IXRSelectInteractable interactableObject, bool shallWaitForEase = true) 
        {
            interactableObject.transform.SetParent(_transform);
            CurrentBulletCount++;
            RegisterBullet(interactableObject);

            XRGrabInteractable grabInteractable = interactableObject.transform.GetComponent<XRGrabInteractable>();
            Rigidbody grabInteractableRigidbody = grabInteractable.gameObject.GetComponent<Rigidbody>();

            if (shallWaitForEase)
                await UniTask.WaitForSeconds(grabInteractable.attachEaseInTime);

            if (_loadedBullets.Count(loadedBullet => loadedBullet != null) > 1 && 
                _topBullet != null)
            {
                _topBullet.SetActive(false);
            }

            _topBullet = grabInteractable.transform.gameObject;
            grabInteractable.enabled = false;
            grabInteractableRigidbody.isKinematic = true;
            grabInteractableRigidbody.useGravity = false;

            BulletLoaded?.Invoke();
        }
    }
}
