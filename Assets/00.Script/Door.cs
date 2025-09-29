using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform teleportPos;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !GameManager.instance.isTeleport)
        {
            collision.gameObject.transform.position = teleportPos.position;
            GameManager.instance.Teleport();
        }
    }
}
