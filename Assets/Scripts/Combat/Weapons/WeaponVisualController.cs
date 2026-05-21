using UnityEngine;

namespace TP.Combat.Weapons
{
    public class WeaponVisualController : MonoBehaviour
    {
        [SerializeField] private PlayerWeaponLoadout _loadout;
        [SerializeField] private Transform _equippedSocket;

        private GameObject _currentVisual;

        private void OnEnable()
        {
            if (_loadout == null)
                _loadout = GetComponentInParent<PlayerWeaponLoadout>();

            if (_loadout != null)
            {
                _loadout.EquippedWeaponChanged += RefreshEquippedVisual;
                RefreshEquippedVisual(_loadout.EquippedWeapon);
            }
        }

        private void OnDisable()
        {
            if (_loadout != null)
                _loadout.EquippedWeaponChanged -= RefreshEquippedVisual;
        }

        public void RefreshEquippedVisual(WeaponInstance equippedWeapon)
        {
            ClearVisual();

            var prefab = equippedWeapon?.Definition?.EquippedVisualPrefab;
            if (prefab == null || _equippedSocket == null)
                return;

            _currentVisual = Instantiate(prefab, _equippedSocket);
            _currentVisual.transform.localPosition = Vector3.zero;
            _currentVisual.transform.localRotation = Quaternion.identity;
        }

        public void ClearVisual()
        {
            if (_currentVisual == null)
                return;

            Destroy(_currentVisual);
            _currentVisual = null;
        }
    }
}
