using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRoom(Random.Range(5.0f, 10.0f));
    }

    /*
     * GenerateRoom Function
     * Description: Generates different sizes of rooms in random places
     * Parameters:
     * - float numOfRooms: The number of rooms to be generated.
     * Returns: void.
    */
    void GenerateRoom(float numOfRooms)
    {
        for (int i = 0; i < numOfRooms; i++)
        {
            float width = Random.Range(1.0f, 5.0f);
            float height = Random.Range(1.0f, 5.0f);
            float length = Random.Range(1.0f, 5.0f);

            GameObject room = Instantiate(roomPrefab);
            room.transform.localScale = new Vector3(width, height, length);

            // Move the instantiated room to a random position
            float x = Random.Range(-50.0f, 50.0f);
            float z = Random.Range(-50.0f, 50.0f);

            placeRoom(x, z, room);
        }
    }

    void placeRoom(float x, float z, GameObject room)
    {
        bool roomOverlapping = false;
        Collider[] colliders = Physics.OverlapBox(new Vector3(x, 0, z), room.transform.localScale / 2, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != room)
            {
                roomOverlapping = true;
                break;
            }
        }

        if (!roomOverlapping)
        {
            room.transform.position = new Vector3(x, 0, z);
        }
        else
        {
            Destroy(room);
        }
    }
}
