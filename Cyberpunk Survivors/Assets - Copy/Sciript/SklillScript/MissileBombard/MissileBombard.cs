using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MissileBombard : MonoBehaviour
{
    [Header("Missile Settings")]
    public GameObject missilePrefab;
    public float bombardmentRadius = 10f;
    public int missileCount = 8;
    public float missileSpawnHeight = 15f;
    public float explosionDelay = 0.5f;

    [Header("Targeting")]
    public LayerMask groundLayer;
    public float targetingRadius = 2f;

    private CameraShake cameraShake;

    private void Start()
    {
        cameraShake = FindObjectOfType<CameraShake>();
        if (cameraShake == null)
            Debug.LogError("CameraShake not found in scene!");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 pos = player.GetComponent<Transform>().position;
            FireMissiles(pos);
        }
    }

    public void FireMissiles(Vector3 centerPosition)
    {
        for (int i = 0; i < missileCount; i++)
        {
            float randomAngle = Random.Range(0f, 360f);
            float randomRadius = Random.Range(0f, bombardmentRadius);
            float x = centerPosition.x + randomRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float z = centerPosition.z + randomRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

            if (Physics.Raycast(new Vector3(x, 1000f, z), Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                Vector3 spawnPos = new Vector3(x, missileSpawnHeight, z);
                Vector3 targetPos = hit.point;
                StartCoroutine(SpawnMissile(spawnPos, targetPos, i * explosionDelay));
            }
        }
    }

    System.Collections.IEnumerator SpawnMissile(Vector3 spawnPos, Vector3 targetPos, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject missile = Instantiate(missilePrefab, spawnPos, Quaternion.identity);
        Missile missileComponent = missile.GetComponent<Missile>();
        missileComponent.Initialize(targetPos, targetingRadius);
        missileComponent.onExplode += OnMissileExplode; // 이벤트 구독
    }

    private void OnMissileExplode()
    {
        if (cameraShake != null)
            cameraShake.ShakeCamera();
    }
}