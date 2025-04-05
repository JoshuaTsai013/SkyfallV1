using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;


public class GunReload : MonoBehaviour
{
    private PlayerStats _playerStats;
    [Range(0, 5)]
    [SerializeField]
    private float _reloadTime = 2f; // Time taken to reload in seconds
    private void Start()
    {
        // Get the PlayerStats component from the PlayerManager instance
        _playerStats = PlayerManager.instance.playerStats;
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats component not found on PlayerManager instance.");
        }
    }
    public void Reload()
    {
        // Check if the player has ammo and if the magazine is not full
        if (_playerStats.AmmoAmount >= _playerStats.MagazineCapacity || _playerStats.MaxAmmoAmount <= 0 || _playerStats.IsReloading)
        {
            // Play empty sound
            // SoundManager.PlaySound(SoundType.Empty, 0.1f);
            return;
        }
        //Start reloading
        StartRelaod(destroyCancellationToken).Forget();
    }
    async UniTask StartRelaod(CancellationToken cancellation)
    {

        // Play reload sound
        // SoundManager.PlaySound(SoundType.Reload, 0.1f);
        _playerStats.MaxAmmoAmount += _playerStats.AmmoAmount;
        _playerStats.AmmoAmount = 0;
        _playerStats.IsReloading = true;
        // await UniTask.Delay((int)(_reloadTime * 1000), cancellationToken: cancellation);
        if (_playerStats.MaxAmmoAmount < _playerStats.MagazineCapacity)
        {
            int ammoToReload = _playerStats.MaxAmmoAmount;
            // int initialAmmo = _playerStats.MaxAmmoAmount;
            while (_playerStats.MaxAmmoAmount > 0)
            {
                _playerStats.MaxAmmoAmount--;
                await UniTask.Delay((int)(_reloadTime * 1000 / ammoToReload), cancellationToken: cancellation);
            }
            await UniTask.Delay(500, cancellationToken: cancellation);
            _playerStats.AmmoAmount = ammoToReload;
        }
        else
        {
            int ammoCount = _playerStats.MagazineCapacity;
            while (ammoCount > 0)
            {
                ammoCount--;
                _playerStats.MaxAmmoAmount--;
                await UniTask.Delay((int)(_reloadTime * 1000 / _playerStats.MagazineCapacity), cancellationToken: cancellation);
            }
            await UniTask.Delay(500, cancellationToken: cancellation);
            _playerStats.AmmoAmount = _playerStats.MagazineCapacity;
        }
        _playerStats.IsReloading = false;

    }
}
