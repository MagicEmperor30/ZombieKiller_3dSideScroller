using System;
using UnityEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }
    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position,targetCamPos,smoothing * Time.deltaTime);
    }
}
