using UnityEngine;
using XRTest.Utilities;

namespace XRTest.Weapons.Bullets 
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public abstract class Bullet : MonoBehaviour, IMonoBehaviourPoolElement
    {
        [SerializeField] private float _damageModifier;
        [SerializeField] private float _shotImpulse;

        private Rigidbody _rigidbody;
        private bool _isFlying = false;
        private float _timer;
        private Vector3 _currentFireline;

        public GameObject GameObject { get; private set; }
        public Transform Transform { get; private set; }

        public void Awake()
        {
            GameObject = gameObject;
            Transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void StartFlying(Quaternion rotation, Vector3 position, Vector3 fireline)
        {
            GameObject.SetActive(true);
            Transform.SetPositionAndRotation(position, rotation);
            _currentFireline = fireline;
            _rigidbody.velocity = Vector3.zero;
            _isFlying = true;
        }

        private void Update()
        {
            if (_isFlying == false)
                return;

            _rigidbody.velocity = _currentFireline * _shotImpulse;
            _timer += Time.deltaTime;

            if (_timer > 5f)
                EndFlying();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_isFlying == false)
                return;

            EndFlying();
        }

        protected virtual void EndFlying()
        {
            GameObject.SetActive(false);
            _isFlying = false;
            _timer = 0f;
        }
    }
}