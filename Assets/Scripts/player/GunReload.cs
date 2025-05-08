using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class GunReload : MonoBehaviour
{
    private PlayerStats _playerStats;
    [SerializeField] private GameObject _ReloadHintText; //RepairKit text object

    private void Start()
    {
        // Get the PlayerStats component from the PlayerManager instance
        _playerStats = PlayerManager.instance.playerStats;
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats component not found on PlayerManager instance.");
        }
    }
    private void FixedUpdate()
    {
        if (_playerStats.AmmoAmount < 10 && !_playerStats.IsReloading)
        {
            _ReloadHintText.SetActive(true);
        }
    }
    public void Reload()
    {
        // Check if the player has ammo and if the magazine is not full
        if (_playerStats.AmmoAmount >= _playerStats.MagazineCapacity || _playerStats.MaxAmmoAmount <= 0 || _playerStats.IsReloading)
        {
            return;
        }
        SoundManager.PlaySound(SoundType.Reload, 0.4f);
        // Start reloading
        StartReload(destroyCancellationToken).Forget();
        _ReloadHintText.SetActive(false);
    }

    async UniTask StartReload(CancellationToken cancellation)
    {
        _playerStats.IsReloading = true;
        int ammoToReload;
        int ammoCounter;

        if (_playerStats.MaxAmmoAmount + _playerStats.AmmoAmount < _playerStats.MagazineCapacity)
        {
            ammoToReload = _playerStats.MaxAmmoAmount;
            ammoCounter = ammoToReload;

            while (ammoCounter > 0)
            {
                ammoCounter--;
                _playerStats.MaxAmmoAmount--;
                await UniTask.Delay((int)(_playerStats.ReloadTime * 1000 / ammoToReload), cancellationToken: cancellation);
            }
            _playerStats.AmmoAmount += ammoToReload;
        }
        else
        {
            ammoToReload = _playerStats.MagazineCapacity - _playerStats.AmmoAmount;
            ammoCounter = ammoToReload;

            while (ammoCounter > 0)
            {
                ammoCounter--;
                _playerStats.MaxAmmoAmount--;
                await UniTask.Delay((int)(_playerStats.ReloadTime * 1000 / ammoToReload), cancellationToken: cancellation);
            }

            _playerStats.AmmoAmount = _playerStats.MagazineCapacity;
        }

        _playerStats.IsReloading = false;
    }
}
