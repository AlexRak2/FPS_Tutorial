using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunDefinition gunDefinition;
    public int currentAmmo;
    public int currentTotalAmmo;

    public ParticleSystem muzzleFx;
 
    private WeaponHandler weaponHandler;

    public Animator anim { get; private set; }

    private void Start()
    {
        anim = GetComponent<Animator>();
        weaponHandler = GetComponentInParent<WeaponHandler>();

        InitializeGun();
    }

    private void InitializeGun() 
    {
        currentAmmo = gunDefinition.ClipSize;
        currentTotalAmmo = gunDefinition.MaxAmmo;
    }

    public void ReloadGunEvent() 
    {
        weaponHandler.ReloadGun();
    }
}
