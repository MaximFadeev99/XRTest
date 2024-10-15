using XRTest.Interfaces;
using XRTest.Signals;
using XRTest.Weapons;
using Zenject;

namespace XRTest.MainPlayer
{
    internal class CombatAspect
    {
        private readonly SignalBus _signalBus;

        private IInterruptable _leftInterruptableWeapon;
        private IInterruptable _rightInterruptableWeapon;

        public Weapon EquippedLeftWeapon { get; private set; }
        public RangedWeapon EquippedLeftRangedWeapon { get; set; }

        public Weapon EquippedRightWeapon { get; private set; }
        public RangedWeapon EquippedRightRangedWeapon { get; set; }

        internal CombatAspect(SignalBus signalBus) 
        {
            _signalBus = signalBus;
        }

        internal void EquipWeaponOnLeft(Weapon weaponToEquip)
        {
            if (weaponToEquip == null)
                return;

            EquippedLeftWeapon = weaponToEquip;

            if (EquippedLeftWeapon is RangedWeapon rangedWeapon)
            {
                EquippedLeftRangedWeapon = rangedWeapon;
                SetSubscriptionsForRangedWeapon(EquippedLeftRangedWeapon, false);

                if (EquippedLeftRangedWeapon.CheckIfHasMagazineInserted(out int currentBulletCount,
                    out int maxBulletCount)) 
                {
                    OnMagazineInserted(maxBulletCount, false);
                    OnBulletCountChanged(currentBulletCount, false);
                }
            }

            if (EquippedLeftRangedWeapon is IInterruptable interruptableWeapon) 
                _leftInterruptableWeapon = interruptableWeapon;
        }

        internal void UnequipWeaponOnLeft() 
        {
            if (EquippedLeftRangedWeapon != null)
            {
                RemoveSubscriptionsForRangedWeapon(EquippedLeftRangedWeapon, false);
                OnMagazineRemoved(false);
            }

            EquippedLeftWeapon = null;
            EquippedLeftRangedWeapon = null;
        }

        internal void EquipWeaponOnRight(Weapon weaponToEquip)
        {
            if (weaponToEquip == null)
                return;

            EquippedRightWeapon = weaponToEquip;

            if (EquippedRightWeapon is RangedWeapon rangedWeapon)
            {
                EquippedRightRangedWeapon = rangedWeapon;
                SetSubscriptionsForRangedWeapon(EquippedRightRangedWeapon, true);

                if (EquippedRightRangedWeapon.CheckIfHasMagazineInserted(out int currentBulletCount,
                    out int maxBulletCount))
                {
                    OnMagazineInserted(maxBulletCount, true);
                    OnBulletCountChanged(currentBulletCount, true);
                }
            }

            if (EquippedRightRangedWeapon is IInterruptable interruptableWeapon)
                _rightInterruptableWeapon = interruptableWeapon;
        }

        internal void UnequipWeaponOnRight() 
        {
            if (EquippedRightRangedWeapon != null)
            {           
                RemoveSubscriptionsForRangedWeapon(EquippedRightRangedWeapon, true);
                OnMagazineRemoved(true);
            }

            EquippedRightWeapon = null;
            EquippedRightRangedWeapon = null;
        }

        internal void TryUseRightWeapon() 
        {
            if (EquippedRightWeapon != null)
                EquippedRightWeapon.Use();
        }

        internal void TryUseLeftWeapon() 
        {
            if (EquippedLeftWeapon != null)
                EquippedLeftWeapon.Use();
        }

        internal void StopUsingRightWeapon() 
        {
            _rightInterruptableWeapon?.Interrupt();
        }

        internal void StopUsingLeftWeapon()
        {
            _leftInterruptableWeapon?.Interrupt();
        }

        private void SetSubscriptionsForRangedWeapon(RangedWeapon rangedWeapon, bool isEquippedOnRight) 
        {
            rangedWeapon.MagazineInserted += (int maxBulletCount) => 
                OnMagazineInserted(maxBulletCount, isEquippedOnRight);
            rangedWeapon.MagazineRemoved += () => OnMagazineRemoved(isEquippedOnRight);
            rangedWeapon.BulletCountChanged += (int currentBulletCount) => 
                OnBulletCountChanged(currentBulletCount, isEquippedOnRight);
        }

        private void RemoveSubscriptionsForRangedWeapon(RangedWeapon rangedWeapon, bool isEquippedOnRight)
        {
            //Bad code
            rangedWeapon.MagazineInserted = null;
            rangedWeapon.MagazineRemoved = null;
            rangedWeapon.BulletCountChanged = null;
        }

        private void OnMagazineInserted(int maxBulletCount, bool isInsertedOnRight) 
        {
            _signalBus.Fire(new MagazineInsertedSignal(maxBulletCount, isInsertedOnRight));
        }

        private void OnMagazineRemoved(bool isRemovedOnRight) 
        {
            _signalBus.Fire(new MagazineRemovedSignal(isRemovedOnRight));
        }

        private void OnBulletCountChanged(int currentBulletCount, bool hasChangedOnRight) 
        {
            _signalBus.Fire(new AmmoCountChangedSignal(currentBulletCount, hasChangedOnRight));
        }
    }
}