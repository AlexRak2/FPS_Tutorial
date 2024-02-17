using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using UnityEngine;

public class WeaponHandler : NetworkBehaviour
{
    [SerializeField] private LayerMask shootableLayer;
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

    private CameraRecoil camRecoil;
    [SerializeField] private ADS ads;
    [SerializeField] private Camera cam;
    [SerializeField] private WeaponSway weaponSway;
    public bool CanShoot() => currentGun && currentGun.currentAmmo > 0 && !isReloading && !currentGun.anim.GetBool("IsEquipping") && fireTime <= 0;
    public bool CanReload() => currentGun && (currentGun.currentAmmo < currentGun.gunDefinition.ClipSize && currentGun.currentTotalAmmo > 0);
    public bool IsAiming() => Input.GetMouseButton(1);

    bool isReloading;
    float fireTime;

    
    public override void OnStartServer()
    {
        base.OnStartServer();

        //temporary untill spawning default weapon logic
        EquipGunSlot(0, 0);
        EquipGunSlot(1, 1);
    }

    private void Start()
    {
        camRecoil = GetComponent<CameraRecoil>();
    }

    private void Update()
    {

        HandleFiring();
        HandleReload();

        #region Equip Handling
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (secondaryGun && currentGun != secondaryGun)
                CmdChangeCurrentIndex(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            if(primaryGun && currentGun != primaryGun)
               CmdChangeCurrentIndex(1);
        }
        #endregion
    }

    private void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && CanReload()) 
        {
            currentGun.anim.SetBool("IsReloading", true);
            isReloading = true;
        }
    }

    public void ReloadGun() 
    {

        int ammoToAdd = (int)Mathf.Min(currentGun.gunDefinition.ClipSize - currentGun.currentAmmo, currentGun.currentTotalAmmo);

        currentGun.currentAmmo += ammoToAdd;
        currentGun.currentTotalAmmo -= ammoToAdd;

        currentGun.anim.SetBool("IsReloading", false);
        isReloading = false;
    }

    private void HandleFiring()
    {
        if (fireTime > 0)
            fireTime -= Time.deltaTime;

        switch (currentGun.gunDefinition.FiringType)
        {
            case FiringType.Auto:
                AutoFire();
                break;
            case FiringType.Semi:
                SemiAutoFire();
                break;
        }
    }

    private void SemiAutoFire() 
    {
        if (Input.GetMouseButtonDown(0) && CanShoot())
        {
            fireTime = 60 / currentGun.gunDefinition.roundsPerMinute;
            AttemptFire();
        }
    }

    private void AutoFire()
    {
        if (Input.GetMouseButton(0) && CanShoot())
        {
            fireTime = 60 / currentGun.gunDefinition.roundsPerMinute;
            AttemptFire();
        }
    }


    private void AttemptFire()
    {
        currentGun.anim.SetTrigger("Fire");
        currentGun.currentAmmo--;
        currentGun.muzzleFx.Play();

        camRecoil.RecoilFire();

        FireRaycast();
    }

    [SerializeField] private GameObject hitFx;
    public float bulletSpeed;
    private void FireRaycast()
    {
        Vector3 origin = cam.transform.position;
        Vector3 direction = GetDirection();

        TrailRenderer trail = Instantiate(currentGun.gunDefinition.bulletTracer, currentGun.muzzleFx.transform.position, Quaternion.identity).GetComponent<TrailRenderer>();

        if (Physics.Raycast(origin, direction, out RaycastHit hit, Mathf.Infinity, shootableLayer))
        {
            StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));
        }
        else
        {
            Vector3 fallbackPoint = currentGun.muzzleFx.transform.position + GetDirection() * 100;
            StartCoroutine(SpawnTrail(trail, fallbackPoint, Vector3.zero, false));
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal, bool madeImpact)
    {
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(startPosition, hitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, 1 - (remainingDistance / distance));
            remainingDistance -= bulletSpeed * Time.deltaTime;
            yield return null;
        }

        trail.transform.position = hitPoint;

        if (madeImpact)
        {
            GameObject impactFX = Instantiate(hitFx, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitNormal));
            Destroy(impactFX, 2f);
        }

        Destroy(trail.gameObject, trail.time);
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = cam.transform.forward;

        direction += new Vector3(
            Random.Range(-currentGun.gunDefinition.BulletSpread.x, currentGun.gunDefinition.BulletSpread.x),
            Random.Range(-currentGun.gunDefinition.BulletSpread.y, currentGun.gunDefinition.BulletSpread.y),
            Random.Range(-currentGun.gunDefinition.BulletSpread.z, currentGun.gunDefinition.BulletSpread.z));

        direction.Normalize();


        return direction;
    }

    [ServerRpc]
    private void CmdChangeCurrentIndex(byte index) 
    {
        currentGunIndex = index;
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

        weaponSway.SetUp(currentGun);
        ads.SetUp(currentGun);
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
