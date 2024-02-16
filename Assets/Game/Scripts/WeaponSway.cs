using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Position Sway")]
    [SerializeField] private float intensity;
    [SerializeField] private float maxAmount;
    [SerializeField] private float smoothAmount;

    [Header("Rotation Sway")]
    [SerializeField] private float rotationAmount;
    [SerializeField] private float maxRotationAmount;
    [SerializeField] private float smoothRotation;

    [Header("Tilt Sway")]
    [SerializeField] private float tiltAmount;
    [SerializeField] private float maxTiltAmount;
    [SerializeField] private float smoothTiltAmount;

    [SerializeField] private Transform pivot;
    [SerializeField] private Transform tiltPivot;

    Vector3 initialPosition;
    Quaternion initialRotation;

    private void Start()
    {
        initialPosition = pivot.localPosition;
        initialRotation = pivot.localRotation;
    }

    private bool AllowSway()
    {
        return true;
    }

    private void Update()
    {
        float InputX = AllowSway() ? -Input.GetAxis("Mouse X") : 0;
        float InputY = AllowSway() ? -Input.GetAxis("Mouse Y") : 0;

        PositionalSway(InputX, InputY);
        RotateSway(InputX, InputY);
        Tilt();
    }

    private void PositionalSway(float InputX, float InputY)
    {
        float moveX = Mathf.Clamp(InputX * intensity, 0f - maxAmount, maxAmount);
        float moveY = Mathf.Clamp(InputY * intensity, 0f - maxAmount, maxAmount);
        Vector3 vector = new Vector3(moveX, moveY, 0f);
        pivot.localPosition = Vector3.Lerp(pivot.localPosition, vector + initialPosition, Time.deltaTime * smoothAmount);
    }

    private void RotateSway(float InputX, float InputY)
    {
        float rotateX = Mathf.Clamp(InputX * rotationAmount, 0f - maxRotationAmount, maxRotationAmount);
        float rotateY = Mathf.Clamp(InputY * rotationAmount, 0f - maxRotationAmount, maxRotationAmount);
        Quaternion quaternion = Quaternion.Euler(new Vector3(rotateY, -rotateX, 0));
        pivot.localRotation = Quaternion.Slerp(pivot.localRotation, quaternion * initialRotation, smoothRotation * Time.deltaTime);
    }

    private void Tilt()
    {
        float InputX = AllowSway() ? Input.GetAxis("Horizontal") : 0;
        float InputY = AllowSway() ? Input.GetAxis("Vertical") : 0;

        float rotateX = Mathf.Clamp(InputX * tiltAmount, 0f - maxTiltAmount, maxTiltAmount);
        float rotateY = Mathf.Clamp(InputY * tiltAmount, 0f - maxTiltAmount, maxTiltAmount);
        Quaternion quaternion = Quaternion.Euler(new Vector3(rotateY, 0, -rotateX));
        tiltPivot.localRotation = Quaternion.Slerp(tiltPivot.localRotation, quaternion * initialRotation, smoothTiltAmount * Time.deltaTime);
    }
}
