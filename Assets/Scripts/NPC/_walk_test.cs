using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _walk_test : MonoBehaviour
{
    public float walk_speed;

    void Update()
    {
        transform.Translate(Vector3.right * walk_speed * Time.deltaTime);
    }
}
