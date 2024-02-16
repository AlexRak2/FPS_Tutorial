using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [SerializeField] private Transform cam;
    Vector3 currentRotation, targetRotation;

    private WeaponHandler weaponHandler;

    private void Start()
    {
        weaponHandler = GetComponent<WeaponHandler>();
    }

    private void Update()
    {
        GunDefinition gunDef = weaponHandler.currentGun.gunDefinition;
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, gunDef.returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, gunDef.snappiness * Time.fixedDeltaTime);
        cam.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        GunDefinition gunDef = weaponHandler.currentGun.gunDefinition;
        targetRotation += new Vector3(gunDef.recoilX, Random.Range(-gunDef.recoilY, gunDef.recoilY), Random.Range(-gunDef.recoilZ, gunDef.recoilZ));
    }
}
