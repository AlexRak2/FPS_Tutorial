using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private WeaponHandler weaponHandler;

    Gun gun;
    Transform pivot;
    Transform tiltPivot;
    Vector3 initialPosition;
    Quaternion initialRotation;

    private void Start()
    {
        weaponHandler = GetComponentInParent<WeaponHandler>();

        initialPosition = pivot.localPosition;
        initialRotation = pivot.localRotation;
    }

    private bool AllowSway()
    {
        return true;
    }

    private void Update()
    {
        if (!pivot || !tiltPivot) return;

        float InputX = AllowSway() ? -Input.GetAxis("Mouse X") : 0;
        float InputY = AllowSway() ? -Input.GetAxis("Mouse Y") : 0;

        PositionalSway(InputX, InputY);
        RotateSway(InputX, InputY);
    }

    private void PositionalSway(float InputX, float InputY)
    {
        float intensity = gun.gunDefinition.intensity;
        float maxAmount = gun.gunDefinition.intensity;
        float smoothAmount = gun.gunDefinition.smoothAmount;

        if (weaponHandler.IsAiming())
        {
            intensity *= gun.gunDefinition.posAimMultiplier;
            maxAmount *= gun.gunDefinition.posAimMultiplier;
        }

        float moveX = Mathf.Clamp(InputX * intensity, 0f - maxAmount, maxAmount);
        float moveY = Mathf.Clamp(InputY * intensity, 0f - maxAmount, maxAmount);

        Vector3 vector = new Vector3(moveX, moveY, 0f);
        pivot.localPosition = Vector3.Lerp(pivot.localPosition, vector + initialPosition, Time.deltaTime * smoothAmount);
    }

    private void RotateSway(float InputX, float InputY)
    {
        float rotationAmount = gun.gunDefinition.rotationAmount;
        float maxRotationAmount = gun.gunDefinition.maxRotationAmount;
        float smoothRotation = gun.gunDefinition.smoothRotation;

        if (weaponHandler.IsAiming()) 
        {
            rotationAmount *= gun.gunDefinition.rotAimMultiplier;
            maxRotationAmount *= gun.gunDefinition.rotAimMultiplier;
        }

        float rotateX = Mathf.Clamp(InputX * rotationAmount, 0f - maxRotationAmount, maxRotationAmount);
        float rotateY = Mathf.Clamp(InputY * rotationAmount, 0f - maxRotationAmount, maxRotationAmount);
        Quaternion quaternion = Quaternion.Euler(new Vector3(rotateY, -rotateX, 0));
        pivot.localRotation = Quaternion.Slerp(pivot.localRotation, quaternion * initialRotation, smoothRotation * Time.deltaTime);
    }
    public void SetUp(Gun gun) 
    { 
        this.gun = gun;
        pivot = gun.posPivot.transform;
        tiltPivot = gun.rotPivot.transform;
    }
}
