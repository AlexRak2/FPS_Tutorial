using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : NetworkBehaviour, IDamageable
{
    [SyncVar]
    public float health = 100;

    [Server]
    public void Damage(int amount, byte gunID)
    {
        throw new System.NotImplementedException();
    }
}
