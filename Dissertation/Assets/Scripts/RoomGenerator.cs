using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject corridorPrefab;
    public List<GameObject> rooms = new List<GameObject>();
    private List<GameObject> adjacentRooms = new List<GameObject>();
    private List<GameObject> visitedRooms = new List<GameObject>();
    private IEnumerator coroutine;

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
            Debug.Log("Room position X: " + room.transform.position.x);
            Debug.Log("Room position Z: " + room.transform.position.z);
            room.transform.localScale = new Vector3(width, height, length);

            room.transform.position = new Vector3(x, 0, z);

            Debug.Log("Room position X: " + room.transform.position.x);
            Debug.Log("Room position Z: " + room.transform.position.z);

            rooms.Add(room);
        }

        coroutine = WaitAndDFS(0.1f);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndDFS(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        DFS(rooms[0], visitedRooms, rooms, 1000.0f);

        for (int i = 0; i < visitedRooms.Count - 1; i++)
        {
            ConnectRooms(visitedRooms[i], visitedRooms[i + 1]);
        }

        ConvexHull(rooms);
    }

    /*
     * FindAdjacentRooms Function: Finds adjacent room from current room and adds it to adjacent rooms
     * list. The closest room to the current room is the adjacent room.
     * 
     * Parameters: - GameObject currentRoom: the current room gameobject.
     *             - List<GameObject> roomsList: list of rooms available.
     *             - float maxDistance: the maximum distance to be considered. Rooms further away from this maximum distance
     *                                  will not be connected.
     * Returns: List<GameObject>
     */
    public List<GameObject> FindAdjacentRooms(GameObject currentRoom, List<GameObject> roomsList, float maxDistance)
    {
        List<GameObject> adjacentRoomsList = new List<GameObject>();

        foreach (GameObject room in roomsList)
        {
            if (room != currentRoom)
            {
                float distance = Vector3.Distance(currentRoom.transform.position, room.transform.position);
                Debug.Log("Distance between room " + currentRoom + "and room " + room + "is " + distance);
                if (distance <= maxDistance)
                {
                    adjacentRoomsList.Add(room);
                }
            }
        }

        return adjacentRoomsList;
    }

    void DFS(GameObject currentRoom, List<GameObject> visitedRooms, List<GameObject> adjacentRoomsList, float maxDistance)
    {
        adjacentRooms = FindAdjacentRooms(currentRoom, rooms, maxDistance);

        visitedRooms.Add(currentRoom);

        adjacentRooms.Sort((a, b) => Vector3.Distance(currentRoom.transform.position, a.transform.position).CompareTo(Vector3.Distance(currentRoom.transform.position, b.transform.position)));

        foreach (GameObject room in adjacentRooms)
        {
            if (!visitedRooms.Contains(room))
            {
                DFS(room, visitedRooms, adjacentRooms, maxDistance);
            }
        }
    }

    void ConnectRooms(GameObject startingRoom, GameObject destinationRoom)
    {
        // Delete this out later
        GameObject startingPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
        startingPoint.transform.localScale = new Vector3(1f, 1f, 1f);
        startingPoint.transform.position = startingRoom.transform.position;

        GameObject target = GameObject.CreatePrimitive(PrimitiveType.Cube);
        target.transform.localScale = new Vector3(1f, 1f, 1f);
        target.transform.position = destinationRoom.transform.position;

        /*
         * Calculation of midpoints to determine the spawn point of the corridors.
         * var midpoint refers to the spawnpoint of the corridor if horizontal distance > vertical distance.
         * var midpoint2 refers to the spawnpoint of the corridor if vertical distance > horizontal distance.
         */
        Vector3 midpointX = new Vector3 (((startingRoom.transform.position.x + destinationRoom.transform.position.x) / 2f), 0f, startingRoom.transform.position.z);
        Vector3 midpointX2 = new Vector3(((startingRoom.transform.position.x + destinationRoom.transform.position.x) / 2f), 0f, destinationRoom.transform.position.z);
        float deltaX = (destinationRoom.transform.position.x - startingRoom.transform.position.x);

        Vector3 midpointY = new Vector3 (destinationRoom.transform.position.x, 0f, ((startingRoom.transform.position.z + destinationRoom.transform.position.z) / 2f));
        Vector3 midpointY2 = new Vector3(startingRoom.transform.position.x, 0f, ((startingRoom.transform.position.z + destinationRoom.transform.position.z) / 2f));
        float deltaY = (destinationRoom.transform.position.z - startingRoom.transform.position.z);

        // The turning point of the two generated corridors
        Vector3 connectPoint = new Vector3(destinationRoom.transform.position.x, 0f, startingRoom.transform.position.z);
        Vector3 connectPoint2 = new Vector3(startingRoom.transform.position.x, 0f, destinationRoom.transform.position.z);

        /* 
         * If horizontal distance between two rooms is larger than vertical distance, create horizontal corridor first.
         * Else if vertical distance between two rooms is lager than horizontal distance, create vertical corridor first.
         */
        if ((destinationRoom.transform.position.x - startingRoom.transform.position.x) > 
            (destinationRoom.transform.position.z - startingRoom.transform.position.z))
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
    }

    void ConvexHull(List<GameObject> roomsList)
    {
        List<GameObject> sortedRooms = roomsList;
        sortedRooms.Sort((roomA, roomB) => roomA.transform.position.z.CompareTo(roomB.transform.position.z));
        

        GameObject lowestNode = sortedRooms[0];

        lowestNode.GetComponent<Renderer>().material.color = Color.red;
        sortedRooms[1].GetComponent<Renderer>().material.color = Color.red;

        Vector3 dir = sortedRooms[1].transform.position - lowestNode.transform.position;

        // Calculate the angle between Room B and Room A from Room A's horizontal
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        Debug.Log("Angle between First and Second Room is: " + angle);
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