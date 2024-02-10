using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : NetworkBehaviour
{
    [SerializeField] private GameObject tpMesh, cameraHolder;
    private Movement movement;
    public override void OnStartClient()
    {
        base.OnStartClient();

        movement = gameObject.GetComponent<Movement>();

        if (base.IsOwner)
        {
            movement.SetCursorState(CursorLockMode.Locked, false);
            ToggleTPMesh(false);
        }
        else
        {
            movement.enabled = false;
            cameraHolder.SetActive(false);
            ToggleTPMesh(true);
        }
    }

    private void ToggleTPMesh(bool value)
    {
        tpMesh.SetActive(value);
    }
}
