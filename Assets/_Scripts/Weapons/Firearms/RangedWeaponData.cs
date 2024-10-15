using XRTest.Weapons.Bullets;
using XRTest.Weapons.Magazines;
using UnityEngine;

namespace XRTest.Weapons.Firearms 
{
    [CreateAssetMenu(fileName = "NewRangedWeaponData", menuName = "ProjectData/RangedWeaponData", order = 51)]
    public class RangedWeaponData : WeaponData
    {
        [field: SerializeField] public Bullet BulletPrefab { get; private set; }
        [field: SerializeField] public AudioClip ShootingClip { get; private set; }
        [field: SerializeField] public AudioClip NoAmmoClip { get; private set; }
        [field: SerializeField] public ParticleSystem MuzzleFlashEffect { get; private set; }
        [field: SerializeField] public Magazine CompatibleMagazine { get; private set; }
        //By this number the slider of this weapon must be pulled back in comparison with its initial local z position
        //to finalize a reload
        [field: SerializeField] public float SliderZOffset { get; private set; } = 0.02f;
        [field: SerializeField] public AudioClip SliderPulledSound { get; private set; }
        [field: SerializeField] public BulletCasing BulletCasingPrefab { get; private set; }
    }
}