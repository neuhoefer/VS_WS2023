using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Vector3 offset;

    private void FixedUpdate() {

        transform.position = targetTransform.TransformPoint(offset);
        var direction = targetTransform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
