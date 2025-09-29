using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 인스펙터에서 보이게
public class DoorData
{
    public GameObject door;   // 문 프리팹
    public Transform doorPos; // 문이 놓일 위치
}

[System.Serializable]
public class GoldDoorData
{
    public GameObject door;   // 문 프리팹
    public Transform doorPos; // 문이 놓일 위치
}

public class DoorSpawn : MonoBehaviour
{
    public List<DoorData> doors = new List<DoorData>();
    public List<GoldDoorData> GoldDoors = new List<GoldDoorData>();

    private HashSet<Transform> occupiedPositions = new HashSet<Transform>();

    private void Start()
    {
        // 1. 골든 문 먼저 생성
        SpawnRandomGoldDoors();

        // 2. 골든 문 위치를 제외하고 일반 문 생성
        SpawnRandomDoors();
    }

    public void SpawnRandomGoldDoors()
    {
        if (GoldDoors.Count == 0) return;
        if (GameManager.instance.goldenDoorCount >= 1) return;

        int rand = Random.Range(0, GoldDoors.Count);
        GoldDoorData data = GoldDoors[rand];

        if (data.door != null && data.doorPos != null)
        {
            Instantiate(data.door, data.doorPos.position, data.door.transform.rotation);
            GameManager.instance.goldenDoorCount++;

            // 생성된 위치 기록
            occupiedPositions.Add(data.doorPos);
        }
    }

    public void SpawnRandomDoors()
    {
        if (doors.Count == 0) return;

        int doorCount = Random.Range(2, 5);

        // 인덱스 섞기
        List<int> indices = new List<int>();
        for (int i = 0; i < doors.Count; i++) indices.Add(i);
        for (int i = 0; i < indices.Count; i++)
        {
            int rand = Random.Range(i, indices.Count);
            int tmp = indices[i];
            indices[i] = indices[rand];
            indices[rand] = tmp;
        }

        for (int i = 0; i < doorCount && i < indices.Count; i++)
        {
            if (GameManager.instance.nomalDoorCount >= GameManager.instance.maxDoorCount)
                break;

            DoorData data = doors[indices[i]];
            if (data.door != null && data.doorPos != null)
            {
                // 골든 문 위치이면 건너뛰기
                if (occupiedPositions.Contains(data.doorPos))
                    continue;

                Instantiate(data.door, data.doorPos.position, data.door.transform.rotation);
                GameManager.instance.nomalDoorCount++;
            }
        }
    }
}
