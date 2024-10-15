using UnityEngine;

namespace XRTest.Weapons 
{   
    public abstract class WeaponData : ScriptableObject
    {
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }
    }
}