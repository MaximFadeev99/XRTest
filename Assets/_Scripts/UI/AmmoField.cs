using TMPro;
using UnityEngine;

namespace XRTest.UI
{
    public class AmmoField : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentAmmo;
        [SerializeField] private TMP_Text _maxAmmo;

        private GameObject _gameObject;

        internal void Initialize() 
        {
            _gameObject = gameObject;
        }

        internal void Toggle(bool isActive) 
        {
            _gameObject.SetActive(isActive);
        }

        internal void UpdateCurrentAmmo(int currentAmmo) 
        {
            _currentAmmo.text = currentAmmo.ToString();
        }

        internal void UpdateMaxAmmo(int maxAmmo) 
        {
            _maxAmmo.text = maxAmmo.ToString();
        }
    }
}