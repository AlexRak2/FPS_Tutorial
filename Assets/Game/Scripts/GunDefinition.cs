using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FiringType { Auto, Semi }


[CreateAssetMenu(menuName = "Definitions/New Gun")]
public class GunDefinition : ScriptableObject
{
    public int Damage;
    public int MaxAmmo;
    public int ClipSize;
    public float roundsPerMinute;
    public FiringType FiringType;

    [Space(10)]
    public float recoilX;
    public float recoilY;
    public float recoilZ;
    public float snappiness;
    public float returnSpeed;

}
