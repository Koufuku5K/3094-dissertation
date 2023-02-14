using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator2 : MonoBehaviour
{
    public GameObject roomPrefab;
    private int[,] roomsArray;
    private int gridSize = 5;

    // Start is called before the first frame update
    void Start()
    {
        roomsArray = new int[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                /* Initialize each "grid" with a value.
                 * 0 = Room should be created on this "cell"
                 * 1 = Room should not be created on this "cell"
                 */
                roomsArray[i, j] = Random.Range(0, 2);
            }
        }
        GenerateRoom();
    }

    /*
     * GenerateRoom Function
     * Description: Generates rooms in a 2D array.
     * Parameters: None.
     * Returns: void.
    */
    void GenerateRoom()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (roomsArray[i, j] == 0)
                {
                    float x = i * Random.Range(10, 20);
                    float z = j * Random.Range(10, 20);
                    GameObject room = Instantiate(roomPrefab);
                    room.transform.position = new Vector3(x, 0, z);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Room"))
        {
            Debug.Log("Rooms are overlapping");
            // destroy or reposition the room
            Destroy(other.gameObject);
            Debug.Log("Room Destroyed");
        }
    }
}
