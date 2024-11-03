using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public List<GameObject> doors; // Assign each door position here in the inspector
    public List<GameObject> characters; // Assign each character here in the inspector
    public float spawnInterval = 2.0f; // Interval between character spawns

    private void Start()
    {
        StartCoroutine(SpawnCharacters());
    }

    private IEnumerator SpawnCharacters()
    {
        while (true)
        {
            ActivateRandomCharacter();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void ActivateRandomCharacter()
    {
        // Find an inactive character to activate
        GameObject character = characters.Find(c => !c.activeInHierarchy);

        if (character != null)
        {
            // Randomly select start and end doors
            Transform startDoor = doors[Random.Range(0, doors.Count)].transform;
            Transform endDoor = doors[Random.Range(0, doors.Count)].transform;

            // Ensure the start and end doors are different
            while (endDoor == startDoor)
            {
                endDoor = doors[Random.Range(0, doors.Count)].transform;
            }

            // Initialize and activate character
            character.GetComponent<NPCWalk>().Initialize(startDoor, endDoor);
        }
    }
}
