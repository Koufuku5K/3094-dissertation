using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject corridorPrefab;
    public List<GameObject> rooms = new List<GameObject>();
    private List<GameObject> visited = new List<GameObject>();
    private List<GameObject> unvisited = new List<GameObject>();

    private CheckRoomOverlap checkRoomOverlap;
   
    // Start is called before the first frame update
    void Start()
    {
        GenerateRoom(Random.Range(10.0f, 15.0f));
    }

    /*
     * GenerateRoom Function: Generates rooms of random sizes, 
     * in random locations, and adds the generated room into rooms list.
     * 
     * Parameters: - float numOfRooms: the number of rooms to be generated (randomly picked between 10 and 14).
     * Returns: void
     */
    void GenerateRoom(float numOfRooms)
    {
        Debug.Log(numOfRooms);

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

            rooms.Add(room);
        }

        ConnectRooms(rooms[0], rooms[1]);
    }

    void ConnectRooms(GameObject startingRoom, GameObject destinationRoom)
    {
        // Calculate the midpoint between the two rooms
        Vector3 midpoint = (startingRoom.transform.position + destinationRoom.transform.position) / 2f;

        // Calculate the distance between the two rooms
        float distance = Vector3.Distance(startingRoom.transform.position, destinationRoom.transform.position);

        // Create a corridor as a thin, rectangular cube
        GameObject corridor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        corridor.transform.localScale = new Vector3(10f, 0.1f, distance);
        corridor.transform.position = midpoint;

        // Rotate the corridor to face the direction of roomB
        Vector3 direction = destinationRoom.transform.position - startingRoom.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        corridor.transform.rotation = rotation;

        //GameObject corridor = Instantiate(corridorPrefab);
        //corridor.transform.localScale = new Vector3(Mathf.Abs(roomA.transform.localScale.x - roomB.transform.localScale.x), 0, Mathf.Abs(roomA.transform.localScale.z - roomB.transform.localScale.z)   );
    }

    /*
     * RemoveRoom Function: Removes room from rooms list if the room was destroyed.
     * Parameter: - GameObject room: The room GameObject to be removed from the rooms list.
     * Returns: void.
     */
    public void RemoveRoom(GameObject room)
    {
        rooms.Remove(room);
    }
}