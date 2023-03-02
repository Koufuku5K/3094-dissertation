using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject corridorPrefab;
    public List<GameObject> rooms = new List<GameObject>();
    //private List<GameObject> visited = new List<GameObject>();
    //private List<GameObject> unvisited = new List<GameObject>();

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
            float width = Random.Range(2.0f, 5.0f);
            float height = Random.Range(2.0f, 5.0f);
            float length = Random.Range(2.0f, 5.0f);

            // Determine the position of the room
            float x = Random.Range(-100.0f, 100.0f);
            float z = Random.Range(-100.0f, 100.0f);

            GameObject room = Instantiate(roomPrefab);
            room.transform.localScale = new Vector3(width, height, length);

            room.transform.position = new Vector3(x, 0, z);

            rooms.Add(room);
        }

        ConnectRooms(rooms[0], rooms[1]);
        ConnectRooms(rooms[1], rooms[2]);
        ConnectRooms(rooms[2], rooms[3]);
    }

    void ConnectRooms(GameObject startingRoom, GameObject destinationRoom)
    {
        GameObject startingPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
        startingPoint.transform.localScale = new Vector3(1f, 1f, 1f);
        startingPoint.transform.position = startingRoom.transform.position;

        GameObject target = GameObject.CreatePrimitive(PrimitiveType.Cube);
        target.transform.localScale = new Vector3(1f, 1f, 1f);
        target.transform.position = destinationRoom.transform.position;

        /*// Calculate the midpoint between the two rooms
        Vector3 midpoint = (startingRoom.transform.position + destinationRoom.transform.position) / 2f;

        // Calculate the distance between the two rooms
        float distance = Vector3.Distance(startingRoom.transform.position, destinationRoom.transform.position);

        // Create a corridor as a cube
        GameObject corridor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        corridor.transform.localScale = new Vector3(10f, 0.1f, distance);
        corridor.transform.position = midpoint;

        // Rotate the corridor to face the direction of destinationRoom
        Vector3 direction = destinationRoom.transform.position - startingRoom.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        corridor.transform.rotation = rotation;*/

        Vector3 midpointX = new Vector3 (((startingRoom.transform.position.x + destinationRoom.transform.position.x) / 2f), 0f, startingRoom.transform.position.z);
        Vector3 midpointX2 = new Vector3(((startingRoom.transform.position.x + destinationRoom.transform.position.x) / 2f), 0f, destinationRoom.transform.position.z);
        float deltaX = (destinationRoom.transform.position.x - startingRoom.transform.position.x);
        Vector3 midpointY = new Vector3 (destinationRoom.transform.position.x, 0f, ((startingRoom.transform.position.z + destinationRoom.transform.position.z) / 2f));
        Vector3 midpointY2 = new Vector3(startingRoom.transform.position.x, 0f, ((startingRoom.transform.position.z + destinationRoom.transform.position.z) / 2f));
        float deltaY = (destinationRoom.transform.position.z - startingRoom.transform.position.z);
        Vector3 connectPoint = new Vector3(destinationRoom.transform.position.x, 0f, startingRoom.transform.position.z);
        Vector3 connectPoint2 = new Vector3(startingRoom.transform.position.x, 0f, destinationRoom.transform.position.z);

        if ((destinationRoom.transform.position.x - startingRoom.transform.position.x) > (destinationRoom.transform.position.z - startingRoom.transform.position.z))
        {
            // Create a corridor as a cube
            GameObject corridorX = GameObject.CreatePrimitive(PrimitiveType.Cube);
            corridorX.transform.localScale = new Vector3(10f, 0.1f, deltaX);
            corridorX.transform.position = midpointX;

            // Rotate the corridor to face the direction of destinationRoom
            Vector3 directionX = new Vector3(destinationRoom.transform.position.x - startingRoom.transform.position.x, 0f, 0f);
            Quaternion rotationX = Quaternion.LookRotation(directionX, Vector3.up);
            corridorX.transform.rotation = rotationX;

            GameObject corridorY = GameObject.CreatePrimitive(PrimitiveType.Cube);
            corridorY.transform.localScale = new Vector3(10f, 0.1f, deltaY);
            corridorY.transform.position = midpointY;

            // Rotate the corridor to face the direction of destinationRoom
            Vector3 directionY = new Vector3(0f, 0f, destinationRoom.transform.position.z - startingRoom.transform.position.z);
            Quaternion rotationY = Quaternion.LookRotation(directionY, Vector3.up);
            corridorY.transform.rotation = rotationY;

            GameObject corridorConnect = GameObject.CreatePrimitive(PrimitiveType.Cube);
            corridorConnect.transform.localScale = new Vector3(10f, 0.1f, 10f);
            corridorConnect.transform.position = connectPoint;
        }
        else if ((destinationRoom.transform.position.z - startingRoom.transform.position.z) > (destinationRoom.transform.position.x - startingRoom.transform.position.x))
        {
            GameObject corridorY = GameObject.CreatePrimitive(PrimitiveType.Cube);
            corridorY.transform.localScale = new Vector3(deltaY, 0.1f, 10f);
            corridorY.transform.position = midpointY2;

            // Rotate the corridor to face the direction of destinationRoom
            Vector3 directionY = new Vector3(destinationRoom.transform.position.z - startingRoom.transform.position.z, 0f, 0f);
            Quaternion rotationY = Quaternion.LookRotation(directionY, Vector3.up);
            corridorY.transform.rotation = rotationY;

            // Create a corridor as a cube
            GameObject corridorX = GameObject.CreatePrimitive(PrimitiveType.Cube);
            corridorX.transform.localScale = new Vector3(deltaX, 0.1f, 10f);
            corridorX.transform.position = midpointX2;

            // Rotate the corridor to face the direction of destinationRoom
            Vector3 directionX = new Vector3(0f, 0f, destinationRoom.transform.position.x - startingRoom.transform.position.x);
            Quaternion rotationX = Quaternion.LookRotation(directionX, Vector3.up);
            corridorX.transform.rotation = rotationX;

            GameObject corridorConnect = GameObject.CreatePrimitive(PrimitiveType.Cube);
            corridorConnect.transform.localScale = new Vector3(10f, 0.1f, 10f);
            corridorConnect.transform.position = connectPoint2;
        }

        /*// Create a corridor as a cube
        GameObject corridorX = GameObject.CreatePrimitive(PrimitiveType.Cube);
        corridorX.transform.localScale = new Vector3(10f, 0.1f, deltaX);
        corridorX.transform.position = midpointX;

        // Rotate the corridor to face the direction of destinationRoom
        Vector3 directionX = new Vector3(destinationRoom.transform.position.x - startingRoom.transform.position.x, 0f, 0f);
        Quaternion rotationX = Quaternion.LookRotation(directionX, Vector3.up);
        corridorX.transform.rotation = rotationX;

        GameObject corridorY = GameObject.CreatePrimitive(PrimitiveType.Cube);
        corridorY.transform.localScale = new Vector3(10f, 0.1f, deltaY);
        corridorY.transform.position = midpointY;

        // Rotate the corridor to face the direction of destinationRoom
        Vector3 directionY = new Vector3(0f, 0f, destinationRoom.transform.position.z - startingRoom.transform.position.z);
        Quaternion rotationY = Quaternion.LookRotation(directionY, Vector3.up);
        corridorY.transform.rotation = rotationY;

        GameObject corridorConnect = GameObject.CreatePrimitive(PrimitiveType.Cube);
        corridorConnect.transform.localScale = new Vector3(10f, 0.1f, 10f);
        corridorConnect.transform.position = connectPoint;*/
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