using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10); 
    [Range(0,10)] public float smoothFactor = 5f;
    public float rotationSpeed = 5f;

    private Vector3 currentOffset;
    private Quaternion targetRotation;

    private void Start()
    {
        currentOffset = offset; 
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        Vector3 targetPosition = target.position + currentOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.deltaTime);
    }

    private void UpdateRotation()
    {
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void SetOffset(Vector3 newOffset)
    {
        currentOffset = newOffset;
    }
}