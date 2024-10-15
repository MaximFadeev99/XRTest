using System;
using UnityEngine;
using XRTest.Signals;
using Zenject;

namespace XRTest.UI
{
    [Serializable]
    internal class AmmoDrawer
    {
        [SerializeField] private AmmoField _leftAmmoField;
        [SerializeField] private AmmoField _rightAmmoField;

        private SignalBus _signalBus;

        internal void Initialize(SignalBus signalBus) 
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<MagazineInsertedSignal>(OnMagazineInserted);
            _signalBus.Subscribe<MagazineRemovedSignal>(OnMagazineRemoved);
            _signalBus.Subscribe<AmmoCountChangedSignal>(OnAmmoCountChanged);

            _leftAmmoField.Initialize();
            _rightAmmoField.Initialize();
        }

        internal void Dispose()
        {
            _signalBus.TryUnsubscribe<MagazineInsertedSignal>(OnMagazineInserted);
            _signalBus.TryUnsubscribe<MagazineRemovedSignal>(OnMagazineRemoved);
            _signalBus.TryUnsubscribe<AmmoCountChangedSignal>(OnAmmoCountChanged);
        }

        private void OnMagazineInserted(MagazineInsertedSignal signal)
        {
            AmmoField targetCounter = signal.IsEquippedOnRight ? _rightAmmoField : _leftAmmoField;

            targetCounter.Toggle(true);
            targetCounter.UpdateMaxAmmo(signal.MaxAmmoCount);
        }

        private void OnMagazineRemoved(MagazineRemovedSignal signal)
        {
            AmmoField targetCounter = signal.IsRemovedOnRight ? _rightAmmoField : _leftAmmoField;

            targetCounter.Toggle(false);
        }

        private void OnAmmoCountChanged(AmmoCountChangedSignal signal)
        {
            AmmoField targetCounter = signal.HasChangedOnRight ? _rightAmmoField : _leftAmmoField;

            targetCounter.UpdateCurrentAmmo(signal.CurrentCount);
        }
    }
}