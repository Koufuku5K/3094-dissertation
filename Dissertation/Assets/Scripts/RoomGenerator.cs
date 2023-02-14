using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRoom(Random.Range(10.0f, 15.0f));
    }

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
            float x = Random.Range(-100.0f, 100.0f);
            float z = Random.Range(-100.0f, 100.0f);

            room.transform.position = new Vector3(x, 0, z);
        }
    }
}