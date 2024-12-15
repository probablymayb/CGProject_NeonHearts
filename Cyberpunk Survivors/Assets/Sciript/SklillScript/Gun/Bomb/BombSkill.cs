using UnityEngine;

public class BombSkill : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject bombPrefab;
    public GameObject explosionEffectPrefab;  // 파티클 프리팹 추가

    [Header("Settings")]
    public Transform playerTransform;
    public float initialSpeed = 15f;
    public float launchAngle = 45f;
    public float spreadAngle = 30f;
    public int bombCount = 3;

    void Start()
    {
        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CastBombSkill();
        }
    }

    public void CastBombSkill()
    {
        for (int i = 0; i < bombCount; i++)
        {
            ThrowBomb();
        }
    }

    void ThrowBomb()
    {
        Vector3 startPos = playerTransform.position + Vector3.up * 1.5f;
        float randomSpread = Random.Range(-spreadAngle, spreadAngle);
        Vector3 direction = Quaternion.Euler(0, randomSpread, 0) * playerTransform.forward;

        GameObject bomb = Instantiate(bombPrefab, startPos, Quaternion.identity);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();

        // Bomb 스크립트에 파티클 프리팹 전달
        Bomb bombScript = bomb.GetComponent<Bomb>();
        if (bombScript != null)
        {
            bombScript.explosionEffectPrefab = explosionEffectPrefab;
        }

        if (rb != null)
        {
            float launchRadian = launchAngle * Mathf.Deg2Rad;

            Vector3 velocity = direction * initialSpeed;
            velocity.y = initialSpeed * Mathf.Sin(launchRadian);
            velocity.x = velocity.x * Mathf.Cos(launchRadian);
            velocity.z = velocity.z * Mathf.Cos(launchRadian);

            rb.velocity = velocity;
            rb.useGravity = true;
            rb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
        }
    }
}
