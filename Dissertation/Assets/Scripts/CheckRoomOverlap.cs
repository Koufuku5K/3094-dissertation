using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRoomOverlap : MonoBehaviour
{
    //private RoomGenerator rg;
    private List<GameObject> existingRooms = new List<GameObject>();
    //private List<GameObject> existingRooms = RoomGenerator.rooms;

    /*void Start()
    {
        rg = GetComponent<RoomGenerator>();
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Room"))
        {
            // Check if the room is already in the existingRooms list
            if (!existingRooms.Contains(other.gameObject))
            {
                existingRooms.Add(other.gameObject);

                // Check if the room is colliding with any of the existing rooms
                foreach (GameObject room in existingRooms)
                {
                    if (room != other.gameObject)
                    {
                        if (room.GetComponent<Collider>().bounds.Intersects(other.bounds))
                        {
                            // Destroy the room
                            Debug.Log("Rooms are overlapping");
                            RoomGenerator roomGenerator = FindObjectOfType<RoomGenerator>();
                            /*foreach (var x in roomGenerator.rooms)
                            {
                                Debug.Log(x.ToString());
                            }*/
                            roomGenerator.RemoveRoom(room);
                            //existingRooms.Remove(room);
                            Destroy(room);
                            Debug.Log("Room Destroyed");
                            /*foreach (var x in roomGenerator.rooms)
                            {
                                Debug.Log(x.ToString());
                            }*/
                            Debug.Log("Remaining Rooms: " + roomGenerator.rooms.Count);
                            break;
                        }
                    }
                }
            }
        }
    }
}
