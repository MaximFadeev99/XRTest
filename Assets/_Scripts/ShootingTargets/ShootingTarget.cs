using UnityEngine;
using XRTest.Signals;
using Zenject;

namespace XRTest.ShootingTargets
{
    public class ShootingTarget : MonoBehaviour
    {
        [SerializeField] private CircleHitZone _circleHitZone;

        private SignalBus _signalBus; 

        [Inject]
        private void Construct(SignalBus signalBus) 
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _circleHitZone.HitRegistered += OnHitRegistered;
        }

        private void OnDisable()
        {
            _circleHitZone.HitRegistered -= OnHitRegistered;
        }

        private void OnHitRegistered(int scoreDelta) 
        {
            _signalBus.Fire(new ScoreChangedSignal(scoreDelta));
        }
    }
}