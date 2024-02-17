using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FiringType { Auto, Semi }


[CreateAssetMenu(menuName = "Definitions/New Gun")]
public class GunDefinition : ScriptableObject
{
    [Header("Gun Stats")]
    public int Damage;
    public int MaxAmmo;
    public int ClipSize;
    public float roundsPerMinute;
    public Vector3 BulletSpread;
    public FiringType FiringType;
    public GameObject bulletTracer;

    [Header("Aim")]
    public float aimSpeed;
    public Vector3 aimPosition;
    public Vector3 aimRotation;

    [Header("Camera Recoil")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;
    public float snappiness;
    public float returnSpeed;

    [Header("Position Sway")]
    public float intensity;
    public float maxAmount;
    public float smoothAmount;
    [Range(0f,1f)]
    public float posAimMultiplier = 0.1f;

    [Header("Rotation Sway")]
    public float rotationAmount;
    public float maxRotationAmount;
    public float smoothRotation;
    [Range(0f, 1f)]
    public float rotAimMultiplier = 0.1f;

}
