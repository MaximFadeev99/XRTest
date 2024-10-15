using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XRTest.Weapons.Bullets;

namespace XRTest.ShootingTargets
{
    [RequireComponent(typeof(Collider))]
    internal class CircleHitZone : MonoBehaviour
    {
        [SerializeField] private List<HitArea> _hitAreas;

        private Transform _transform;

        internal Action<int> HitRegistered;

        private void Awake()
        {
            _transform = transform;
        }

        private void OnCollisionEnter(Collision collision)
        {      
            if (collision.gameObject.TryGetComponent(out Bullet collidedBullet) == false)
                return;

            float hitDistance = Vector3.Distance(collision.contacts[0].point, _transform.position);
            HitArea targetArea = _hitAreas
                .Where(hitArea => hitArea.Radius >= hitDistance)
                .OrderBy(hitArea => hitArea.Radius)
                .FirstOrDefault();

            if (targetArea.Points == 0)
                return;

            HitRegistered?.Invoke(targetArea.Points);
        }
    }

    [Serializable]
    internal struct HitArea 
    {
        public int Difficulty;
        public float Radius;
        public int Points;
    }
}