using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    private int[,] roomsArray;
    private int roomGap = 10;
    private int numOfRooms = 5;

    // Start is called before the first frame update
    void Start()
    {
        roomsArray = new int[numOfRooms, numOfRooms];
        for (int i = 0; i < numOfRooms; i++)
        {
            for (int j = 0; j < numOfRooms; j++)
            {
                // 0 means the room is not created, 1 means the room is created
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
        for (int i = 0; i < numOfRooms; i++)
        {
            for (int j = 0; j < numOfRooms; j++)
            {
                if (roomsArray[i, j] == 0)
                {
                    float x = i * roomGap;
                    float z = j * roomGap;
                    GameObject room = Instantiate(roomPrefab);
                    room.transform.position = new Vector3(x, 0, z);
                    roomsArray[i, j] = 1;
                }
            }
        }
    }
}
