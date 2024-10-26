using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;[Range(1, 10)]
    public float smoothFacton;
    public Vector2 edge;
    private void Update()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFacton * Time.deltaTime);
        transform.position = smoothPosition;
    }
    void FixedUpdate()
    {
        
    }
}
