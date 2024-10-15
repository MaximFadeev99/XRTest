using System;
using TMPro;
using UnityEngine;
using XRTest.Signals;
using Zenject;

namespace XRTest.UI
{
    [Serializable]
    internal class ScoreManager
    {
        [SerializeField] private TMP_Text _scoreField;

        private SignalBus _signalBus;
        private int _currentScore = 0;

        internal void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<ScoreChangedSignal>(OnScoreChanged);
            _scoreField.text = "0";
        }

        internal void Dispose()
        {
            _signalBus.TryUnsubscribe<ScoreChangedSignal>(OnScoreChanged);
        }

        private void OnScoreChanged(ScoreChangedSignal scoreChangedSignal)
        {
            _currentScore += scoreChangedSignal.ScoreDelta;
            _scoreField.text = _currentScore.ToString();
        }
    }
}