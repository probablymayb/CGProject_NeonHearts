using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTiling : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float tileSize = 1f;

    void Start()
    {
        PlaceTiles();
    }

    void PlaceTiles()
    {
        Debug.Log("PlaceTiles �޼��� ����");
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 position = new Vector3(x * tileSize, 0, z * tileSize);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                Debug.Log($"Ÿ�� ����: ��ġ {position}");
            }
        }
        Debug.Log($"�� {gridWidth * gridHeight}���� Ÿ�� ���� �Ϸ�");
    }
}
