using UnityEngine;
using Zenject;

namespace XRTest.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private ScoreManager _scoreManager;
        [SerializeField] private AmmoDrawer _ammoDrawer;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus) 
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _scoreManager.Initialize(_signalBus);
            _ammoDrawer.Initialize(_signalBus);          
        }

        private void OnDestroy()
        {
            _scoreManager.Dispose();
            _ammoDrawer.Dispose();
        }
    }
}