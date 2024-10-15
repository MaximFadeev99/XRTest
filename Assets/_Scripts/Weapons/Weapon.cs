using UnityEngine;

namespace XRTest.Weapons 
{
    [RequireComponent(typeof(Animator))]
    public abstract class Weapon : MonoBehaviour
    {
        [field: SerializeField] public Transform AttackPoint { get; private set; }

        protected bool IsOnCooldown = false;
        private float _cooldownTimer = 0f;

        public abstract WeaponData WeaponData { get; }
        public GameObject GameObject { get; private set; }
        protected Animator Animator { get; private set; }

        public virtual void Awake() 
        {
            GameObject = gameObject;
            Animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (IsOnCooldown == false)
                return;

            _cooldownTimer += Time.deltaTime;

            if (_cooldownTimer >= WeaponData.AttackCooldown) 
            {
                IsOnCooldown = false;
                _cooldownTimer = 0f;
            }           
        }

        public abstract void Use();
    }
}