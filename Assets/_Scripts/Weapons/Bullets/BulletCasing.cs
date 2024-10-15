using Cysharp.Threading.Tasks;
using UnityEngine;
using XRTest.Utilities;

namespace XRTest.Weapons.Bullets
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletCasing : MonoBehaviour, IMonoBehaviourPoolElement
    {
        [SerializeField] private float _ejectingPower;

        private Rigidbody _rigidbody;
        private Transform _transform;

        public GameObject GameObject { get; private set; }

        public void Awake()
        {
            GameObject = gameObject;
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
        }

        internal async UniTaskVoid Eject(Transform ejectionPoint, float turnOffTime = 5f) 
        {
            _transform.SetPositionAndRotation(ejectionPoint.position, ejectionPoint.rotation);
            _rigidbody.position = _transform.position;
            _rigidbody.rotation = _transform.rotation;
            GameObject.SetActive(true);

            Vector3 ejectionDirection = Vector3.Lerp(ejectionPoint.up, ejectionPoint.right, Random.Range(0.6f, 1f));

            _rigidbody.AddForce(ejectionDirection * _ejectingPower * Random.Range(0.65f, 1.15f), ForceMode.Impulse);
            _rigidbody.AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

            await UniTask.WaitForSeconds(turnOffTime);

            GameObject.SetActive(false);
            _rigidbody.velocity = Vector3.zero;
        }
    }
}