using UnityEngine;
using XRTest.Utilities;
using XRTest.Interfaces;

namespace XRTest.Weapons.Firearms
{
    public class AK47 : RangedWeapon, IInterruptable
    {
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

            Animator.SetBool(AnimatorParameters.IsShooting, true);
        }

        public override void EjectBullet()
        {
            if (MagazineSocket.CanFire == false) 
            {
                AudioSource.PlayOneShot(RangedWeaponData.NoAmmoClip);
                Interrupt();
                return;
            }

            base.EjectBullet();
        }

        public void Interrupt()
        {
            Animator.SetBool(AnimatorParameters.IsShooting, false);
            IsOnCooldown = true;
        }
    }
}