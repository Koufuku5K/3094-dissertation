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
                            // destroy or reposition the room
                            Debug.Log("Rooms are overlapping");
                            Destroy(room);
                            /*Vector3 newPos = room.transform.position;
                            newPos.x += 10;
                            newPos.z += 10;
                            room.transform.position = newPos;*/
                            break;
                        }
                    }
                }
            }
        }
    }
}
