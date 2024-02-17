using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADS : MonoBehaviour
{
    public bool debugAim;
    private Gun gun;
    private WeaponHandler weaponHandler;

    Vector3 originalPos;
    Quaternion originalRot;

    private void Start()
    {
        weaponHandler = GetComponentInParent<WeaponHandler>();
    }
    private void Update()
    {
        if (!gun) return;

        gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, weaponHandler.IsAiming() || debugAim ? gun.gunDefinition.aimPosition : originalPos, gun.gunDefinition.aimSpeed * Time.deltaTime);
        gun.transform.localRotation = Quaternion.Lerp(gun.transform.localRotation, weaponHandler.IsAiming() || debugAim ? Quaternion.Euler(gun.gunDefinition.aimRotation) : originalRot, gun.gunDefinition.aimSpeed * Time.deltaTime);
    }

    public void SetUp(Gun gun)
    {
        this.gun = gun;
        originalPos = gun.transform.localPosition;
        originalRot = gun.transform.localRotation;
    }
}
