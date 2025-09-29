using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // �ν����Ϳ��� ���̰�
public class DoorData
{
    public GameObject door;   // �� ������
    public Transform doorPos; // ���� ���� ��ġ
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
        // ������ �� ���� (2~4)
        int doorCount = Random.Range(2, 5);

        // �� ����Ʈ �ε����� ����
        List<int> indices = new List<int>();
        for (int i = 0; i < doors.Count; i++) indices.Add(i);

        for (int i = 0; i < indices.Count; i++)
        {
            int rand = Random.Range(i, indices.Count);
            int temp = indices[i];
            indices[i] = indices[rand];
            indices[rand] = temp;
        }

        // �������� ���õ� ���� ����
        for (int i = 0; i < doorCount && i < indices.Count; i++)
        {
            DoorData data = doors[indices[i]];
            if (data.door != null && data.doorPos != null)
            {
                // ������ �����̼� �״�� ���
                Instantiate(data.door, data.doorPos.position, data.door.transform.rotation);
            }
        }
    }
}
