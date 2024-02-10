using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/New Gun")]
public class GunDefinition : ScriptableObject
{
    public int Damage;
    public int MaxAmmo;
    public int ClipSize;
    public int fireRate;

}
