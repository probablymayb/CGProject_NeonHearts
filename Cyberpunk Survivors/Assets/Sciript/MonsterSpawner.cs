using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs;
    public GameObject boss1Prefab;
    public GameObject boss2Prefab;
    public int monsterCount = 30;
    public Vector2 spawnSize;
    public float minDistance = 10.0f;
    public float spawnInterval = 3.0f;
    public float height = 5.0f;
    private List<Vector3> spawnPoss = new List<Vector3>();
    public int killcount = 0;
    public int toBoss = 20;
    private bool bossSpawned1 = false;
    private bool bossSpawned2 = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != null)
        {
            monsterCount = GameManager.Instance.monsterSpawnCount; // 스폰 마릿수 설정
            Debug.Log($"MonsterSpawner 초기화. monsterCount: {monsterCount}, Time.timeScale: {Time.timeScale}");
        }
        else
        {
            Debug.LogWarning("GameManager 인스턴스가 없습니다. 기본값으로 설정합니다.");
        }

        if (Time.timeScale == 0)
        {
            Debug.LogWarning("Time.timeScale이 0으로 설정되어 있어 게임이 멈췄습니다. 1로 초기화합니다.");
            Time.timeScale = 1;
        }

        StartCoroutine(SpawnMonster());
    }


    // Update is called once per frame
    void Update()
    {
        if(Boss_2.boss_1_dead && !bossSpawned2)
        {
            SpawnBoss2();
            bossSpawned2 = true;
        }
    }
    IEnumerator SpawnMonster()
    {
        Debug.Log("SpawnMonster 코루틴 시작");

        for (int i = 0; i < monsterCount; i++)
        {
            Vector3 spawnPos = GetRandomPos();
            GameObject mon = randomIndex();
            Instantiate(mon, spawnPos, Quaternion.identity);
            Debug.Log($"몬스터 스폰 완료. 현재 스폰 수: {i + 1}/{monsterCount}");
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    Vector3 GetRandomPos()
    {
        Vector3 randomPosition;
        do
        {
            float randomX = Random.Range(-spawnSize.x / 2, spawnSize.x / 2);
            float randomZ = Random.Range(-spawnSize.y / 2, spawnSize.y / 2);
            randomPosition = new Vector3(randomX, height, randomZ);
        }
        while (!IsPositionValid(randomPosition));

        spawnPoss.Add(randomPosition);
        return randomPosition;
    }

    bool IsPositionValid(Vector3 position)
    {
        // 기존에 생성된 적들 위치와 비교하여 최소 간격을 확인
        foreach (Vector3 existingPosition in spawnPoss)
        {
            if (Vector3.Distance(existingPosition, position) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
    GameObject randomIndex()
    {
        int random = Random.Range(0, monsterPrefabs.Length);
        return monsterPrefabs[random];
    }
    public void IncreaseKillCount()
    {
        killcount++;
        if (killcount > toBoss && !bossSpawned1)
        {
            SpawnBoss1();
            bossSpawned1 = true;
        }
    }

    void SpawnBoss1()
    {
        Vector3 bossSpawnPos = GetRandomPos();
        Instantiate(boss1Prefab, bossSpawnPos, Quaternion.identity);
        Debug.Log("Boss1 has spawned");
    }

    void SpawnBoss2()
    {
        Vector3 bossSpawnPos = GetRandomPos();
        Instantiate(boss2Prefab, bossSpawnPos, Quaternion.identity);
        Debug.Log("Boss2 has spawned");
    }
}
