using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRoomOverlap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Room"))
        {
            Debug.Log("Room Destroyed");
            Destroy(other.gameObject);
        }
    }
}
