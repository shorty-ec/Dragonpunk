using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWalk : MonoBehaviour
{
    public float speed = 2.0f;
    private Transform startDoor;
    private Transform endDoor;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Transform start, Transform end)
    {
        startDoor = start;
        endDoor = end;
        transform.position = startDoor.position;
        gameObject.SetActive(true);
        StartCoroutine(MoveToDoor());
    }

    private IEnumerator MoveToDoor()
    {
        while (Vector3.Distance(transform.position, endDoor.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endDoor.position, speed * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        for (float alpha = 1.0f; alpha >= 0; alpha -= Time.deltaTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
