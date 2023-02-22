using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMount : MonoBehaviour
{
    [SerializeField]
    private GameObject _singleShot;
    [SerializeField] 
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _weaponFireRate;
    [SerializeField]
    private string _fireKey;

    private float _canFire = 0;
    private bool _tripleShotActive = false;
    private GameObject _activeWeapon;

    void Update()
    {
        FireWeapon();
    }

    private void FireWeapon()
    {
        if (Input.GetKeyDown(_fireKey) && Time.time > _canFire)
        {
            _canFire = Time.time + _weaponFireRate;
            Instantiate(_activeWeapon, transform.position, Quaternion.identity);
        }
    }

    public void SetTripleShotStatus(bool status)
    {
        _tripleShotActive = status;

        if(_tripleShotActive)
        { 
            _activeWeapon = _tripleShotPrefab;
        }
        else
        {
            _activeWeapon = _singleShot;
        }
    }
}
