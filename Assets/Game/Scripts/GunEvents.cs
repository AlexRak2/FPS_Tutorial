using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEvents : MonoBehaviour
{
    private WeaponHandler weaponHandler;

    private void Start()
    {
        weaponHandler = GetComponentInParent<WeaponHandler>();
    }
    public void ReloadGunEvent()
    {
        weaponHandler.ReloadGun();
    }
}
