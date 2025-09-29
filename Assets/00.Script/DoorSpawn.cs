using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 인스펙터에서 보이게
public class DoorData
{
    public GameObject door;   // 문 프리팹
    public Transform doorPos; // 문이 놓일 위치
}

public class DoorSpawn : MonoBehaviour
{
    public List<DoorData> doors = new List<DoorData>();

    private void Start()
    {
        SpawnRandomDoors();


    }
    public void SpawnRandomDoors()
    {
        if (GameManager.instance.nomalDoorCount >= 2) return;
        if (doors.Count == 0) return;

        GameManager.instance.nomalDoorCount++;
        // 생성할 문 개수 (2~4)
        int doorCount = Random.Range(2, 5);

        // 문 리스트 인덱스를 섞기
        List<int> indices = new List<int>();
        for (int i = 0; i < doors.Count; i++) indices.Add(i);

        for (int i = 0; i < indices.Count; i++)
        {
            int rand = Random.Range(i, indices.Count);
            int temp = indices[i];
            indices[i] = indices[rand];
            indices[rand] = temp;
        }

        // 랜덤으로 선택된 문만 생성
        for (int i = 0; i < doorCount && i < indices.Count; i++)
        {
            DoorData data = doors[indices[i]];
            if (data.door != null && data.doorPos != null)
            {
                // 프리팹 로테이션 그대로 사용
                Instantiate(data.door, data.doorPos.position, data.door.transform.rotation);
            }
        }
    }
}
