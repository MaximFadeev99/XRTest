using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using XRTest.Weapons.Bullets;

namespace XRTest.Weapons.Magazines
{
    [RequireComponent(typeof(XRGrabInteractable), typeof(AudioSource))]
    public abstract class Magazine : MonoBehaviour
    {
        [SerializeField] private AmmoSocket _ammoSocket;
        [SerializeField] private Bullet _targetBulletType;
        [SerializeField] private Collider _collider;
        [SerializeField] private int _maxBulletCount;

        private AudioSource _audioSource;
        private XRGrabInteractable _grabInteractable;

        internal int CurrentBulletCount => _ammoSocket.CurrentBulletCount;
        internal int MaxBulletCount => _maxBulletCount;

        private void Awake()
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
            _audioSource = GetComponent<AudioSource>();
            _ammoSocket.Initialize(_maxBulletCount, _targetBulletType.GetType());
            _ammoSocket.BulletLoaded += OnBulletLoaded;
        }

        private void OnEnable()
        {
            _grabInteractable.selectExited.AddListener(OnSelectExited);
        }

        private void OnDisable()
        {
            _grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }

        internal void OnDestroy()
        {
            _ammoSocket.BulletLoaded -= OnBulletLoaded;
        }

        internal void RemoveTopBullet() 
        {
            _ammoSocket.RemoveTopBullet();
        }

        internal void ToggleCollider(bool isEnabled)
        {
            _collider.enabled = isEnabled;
        }

        private void OnBulletLoaded() 
        {
            _audioSource.PlayOneShot(_audioSource.clip);
        }

        private void OnSelectExited(SelectExitEventArgs args) 
        {
            if (args.interactorObject is XRSocketInteractor)
                return;

            ToggleCollider(true);
        }
    }
}