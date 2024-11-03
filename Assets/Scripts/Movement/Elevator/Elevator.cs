using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    NodeManager nodeManager;
    [SerializeField] GameObject elevator;
    [SerializeField] private float elevatorMoveTime = 1.5f;
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.gameObject.SetActive
            NodeManager nodeManager = other.GetComponent<NodeManager>();
            if (nodeManager != null)
            {
                nodeManager.SetElevator(this);
                other.transform.SetParent(transform, true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NodeManager nodeManager = other.GetComponent<NodeManager>();
            if (nodeManager != null)
            {
                nodeManager.ExitElevator();
                other.transform.SetParent(transform, true);
            }
        }
    }

    public void Move(Vector3 position)
    {
        StartCoroutine(MoveCoroutine(position, elevatorMoveTime));
    }

    private IEnumerator MoveCoroutine(Vector3 targetPosition, float time)
    {
        Vector3 startPosition = transform.position; // Get the current position of the elevator
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;  // Calculate normalized time (0 to 1)

            // Apply easing using a simple ease-in-out function (smoothstep for easing in and out)
            t = t * t * (3f - 2f * t);

            // Interpolate between the start position and the target position
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;  // Wait until the next frame
        }

        // Ensure the elevator reaches the exact target position at the end
        transform.position = targetPosition;
    }
}
