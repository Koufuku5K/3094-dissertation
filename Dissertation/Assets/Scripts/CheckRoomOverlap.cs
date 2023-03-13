using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRoomOverlap : MonoBehaviour
{
    private List<GameObject> existingRooms = new List<GameObject>();

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
                            roomGenerator.RemoveRoom(room); 
                            Destroy(room);
                            Debug.Log("Room Destroyed");
                            Debug.Log("Remaining Rooms: " + roomGenerator.rooms.Count);
                            break;
                        }
                    }
                }
            }
        }
    }
}
