using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboad : MonoBehaviour
{
    private Transform CameraTransform;

    private void Start()
    {
        CameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        Quaternion CameraEulerAngle = Quaternion.Euler(CameraTransform.rotation.eulerAngles.x,
            CameraTransform.rotation.eulerAngles.y,
            CameraTransform.rotation.eulerAngles.z);

        transform.LookAt(transform.position + CameraEulerAngle * Vector3.forward,
            CameraTransform.transform.rotation * Vector3.up);
    }
}
