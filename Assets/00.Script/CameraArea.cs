using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraArea : MonoBehaviour
{


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            GameManager.instance.confiner.m_BoundingShape2D = gameObject.GetComponent<PolygonCollider2D>();


    }
}
