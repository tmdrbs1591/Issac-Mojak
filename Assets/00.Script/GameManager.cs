using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CinemachineConfiner2D confiner;

    public int nomalDoorCount;

    public bool isTeleport = false;
    private void Awake()
    {
        instance = this;
    }

    public void Teleport()
    {
        StartCoroutine(Teleport_Cor());
    }
    IEnumerator Teleport_Cor()
    {
        isTeleport = true;
        yield return new WaitForSeconds(0.5f);
        isTeleport = false;
    }
}
