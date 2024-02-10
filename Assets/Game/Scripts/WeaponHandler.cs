using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour
{
    public Gun[] allGuns;

    [Space(10)]
    public Gun currentGun;
    public Gun primaryGun;
    public Gun secondaryGun;

    //0 Secondary 1 Primary
    [SyncVar(OnChange = nameof(OnWeaponIndexChange))]
    private byte currentGunIndex;

    [SyncVar(OnChange = nameof(OnPrimaryIndexChange))]
    private byte primaryGunIndex;

    [SyncVar(OnChange = nameof(OnSecondaryIndexChange))]
    private byte secondaryGunIndex;

    public override void OnStartServer()
    {
        base.OnStartServer();

        EquipGunSlot(0, 0);
    }

    public void EquipGunSlot(byte slotIndex, byte gunID) 
    {
        if(slotIndex == 0)
            secondaryGunIndex = gunID;
        if(slotIndex == 1)
            primaryGunIndex = gunID;
    }

    public void EquipCurrentGun(byte slotIndex) 
    {
        if (currentGun) 
        {
            currentGun.gameObject.SetActive(false);
        }

        if (slotIndex == 0 && secondaryGun)
        {
            secondaryGun.gameObject.SetActive(true);
            currentGun = secondaryGun;
        }
        else if (slotIndex == 1 && primaryGun) 
        {
            primaryGun.gameObject.SetActive(true);
            currentGun = primaryGun;
        }
    }

    #region Sync Hooks
    private void OnWeaponIndexChange(byte oldValue, byte newValue, bool asServer)
    {
        EquipCurrentGun(newValue);
    }

    private void OnPrimaryIndexChange(byte oldValue, byte newValue, bool asServer)
    {
        Gun gun = allGuns[newValue];
        primaryGun = gun;

        //if primary is already equipped equip new gun
        if (currentGunIndex == 1)
            EquipCurrentGun(1);
    }

    private void OnSecondaryIndexChange(byte oldValue, byte newValue, bool asServer)
    {
        Gun gun = allGuns[newValue];
        secondaryGun = gun;

        //if primary is already equipped equip new gun
        if (currentGunIndex == 0)
            EquipCurrentGun(0);
    }

    #endregion
}
