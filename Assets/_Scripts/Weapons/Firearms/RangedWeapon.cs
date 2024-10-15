using XRTest.Utilities;
using XRTest.Weapons.Firearms;
using System;
using UnityEngine;
using Bullet = XRTest.Weapons.Bullets.Bullet;
using UnityEngine.XR.Interaction.Toolkit;
using XRTest.Weapons.Bullets;
using XRTest.Weapons.Magazines;

namespace XRTest.Weapons
{
    [RequireComponent(typeof(AudioSource), typeof(XRGrabInteractable))]
    public abstract class RangedWeapon : Weapon
    {       
        [field: SerializeField] protected Transform BulletContainer { get; private set; }
        [field: SerializeField] protected Transform EjectionPoint { get; private set; }
        [field: SerializeField] internal MagazineSocket MagazineSocket { get; private set; }
        [field: SerializeField] internal Slide Slide { get; private set; }
        [field: SerializeField] public RangedWeaponData RangedWeaponData { get; private set; }

        protected Transform Transform { get; private set; }
        protected XRGrabInteractable GrabInteractable { get; private set; }
        protected AudioSource AudioSource { get; private set; }
        protected MonoBehaviourPool<BulletCasing> BulletCasingPool { get; private set; }
        protected MonoBehaviourPool<Bullet> BulletPool { get; private set; }
        public override WeaponData WeaponData => RangedWeaponData;

        private ParticleSystem _muzzleFlashEffect;

        public Action<int> BulletCountChanged;
        public Action<int> MagazineInserted;
        public Action MagazineRemoved;

        public override void Awake()
        {
            base.Awake();
            Transform = transform;
            AudioSource = GetComponent<AudioSource>();
            GrabInteractable = GetComponent<XRGrabInteractable>();
            BulletPool = new(RangedWeaponData.BulletPrefab, BulletContainer);
            BulletCasingPool = new(RangedWeaponData.BulletCasingPrefab, BulletContainer);
            CreateMuzzleFlashEffect();
            MagazineSocket.Initialize(RangedWeaponData.CompatibleMagazine.GetType());
            Slide.Initialize(Transform, Animator, RangedWeaponData.SliderZOffset);
        }

        private void OnEnable()
        {
            Slide.Pulled += OnSliderPulled;
            MagazineSocket.MagazineInserted += OnMagazineInserted;
            MagazineSocket.MagazineRemoved += OnMagazineRemoved;
            GrabInteractable.selectEntered.AddListener(OnSelectEntered);
            GrabInteractable.selectExited.AddListener(OnSelectExited);
        }

        private void OnDisable()
        {
            Slide.Pulled -= OnSliderPulled;
            MagazineSocket.MagazineInserted -= OnMagazineInserted;
            MagazineSocket.MagazineRemoved -= OnMagazineRemoved;
            GrabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            GrabInteractable.selectExited.RemoveListener(OnSelectExited);
        }

        public override void Use()
        {
            if (IsOnCooldown)
                return;

            if (IsOnCooldown == false &&
                (MagazineSocket.CanFire == false || Slide.IsPulled == false))
            {
                AudioSource.PlayOneShot(RangedWeaponData.NoAmmoClip);
                return;
            }

            Animator.SetTrigger(AnimatorParameters.Shoot);
            IsOnCooldown = true;
        }

        //used by Animator
        public void EjectCapsule()
        {
            BulletCasing idleCasing = BulletCasingPool.GetIdleElement();

            _ = idleCasing.Eject(EjectionPoint);
        }

        //used by Animator
        public virtual void EjectBullet()
        {
            Bullet idleBullet = BulletPool.GetIdleElement();

            idleBullet.StartFlying(AttackPoint.rotation, AttackPoint.position, AttackPoint.forward);
            PlayMuzzleFlashEffect();
            AudioSource.PlayOneShot(RangedWeaponData.ShootingClip);
            MagazineSocket.RemoveBulletFromMagazine();
            BulletCountChanged?.Invoke(MagazineSocket.CurrentBulletCount);
        }

        public bool CheckIfHasMagazineInserted(out int currentBulletCount, out int maxBulletCount) 
        {
            if (MagazineSocket.HasMagazine == false)
            {
                currentBulletCount = 0;
                maxBulletCount = 0;
                return false;
            }
            else 
            {
                currentBulletCount = MagazineSocket.CurrentBulletCount;
                maxBulletCount = MagazineSocket.MaxBulletCount;
                return true;
            }
        }

        private void PlayMuzzleFlashEffect()
        {
            if (_muzzleFlashEffect == null || _muzzleFlashEffect.isStopped)
            {
                CreateMuzzleFlashEffect();
            }

            _muzzleFlashEffect.Play();
        }

        private void CreateMuzzleFlashEffect()
        {
            if (_muzzleFlashEffect != null)
            {
                Destroy(_muzzleFlashEffect.gameObject);
                _muzzleFlashEffect = null;
            }

            _muzzleFlashEffect = Instantiate(RangedWeaponData.MuzzleFlashEffect,
                    AttackPoint.position, AttackPoint.rotation, AttackPoint);
        }

        private void OnSliderPulled()
        {
            AudioSource.PlayOneShot(RangedWeaponData.SliderPulledSound);
        }

        private void OnMagazineInserted(int maxAmmoCount) 
        {
            MagazineInserted?.Invoke(maxAmmoCount);
            BulletCountChanged?.Invoke(MagazineSocket.CurrentBulletCount);
        }

        private void OnMagazineRemoved()
        {
            Slide.ResetPulledProperty();
            MagazineRemoved?.Invoke();
        }

        private void OnSelectEntered(SelectEnterEventArgs _) 
        {
            Slide.ToggleCollider(true);
            MagazineSocket.ToggleSocketInteractor(true);
            MagazineSocket.ToggleMagazineCollider(true);
        }

        private void OnSelectExited(SelectExitEventArgs _) 
        {
            Slide.ToggleCollider(false);
            MagazineSocket.ToggleSocketInteractor(false);
            MagazineSocket.ToggleMagazineCollider(false);
        }
    }
}