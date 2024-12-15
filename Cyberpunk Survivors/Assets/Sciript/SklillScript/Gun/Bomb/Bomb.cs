using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float damageAmount = 50f;
    public GameObject explosionEffectPrefab;
    private bool hasExploded = false;
    private float timer = 0f;  // 타이머 추가
    private float explosionDelay = 1f;  // 폭발 지연 시간

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= explosionDelay && !hasExploded)
        {
            Explode();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter with: {collision.gameObject.name}");
    }


    void Explode()
    {
        // 폭발 이펙트 생성
        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            // 파티클 시스템이 있다면 자동으로 제거되도록 설정
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(effect, ps.main.duration);
            }
        }
        else
        {
            Debug.LogWarning("Explosion effect prefab is not assigned!");
        }

        // 폭발 범위 내 오브젝트 검출 및 데미지 처리
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            //IDamageable damageable = hit.GetComponent<IDamageable>();
            //if (damageable != null)
            //{
            //    // 거리에 따른 데미지 감소
            //    float distance = Vector3.Distance(transform.position, hit.transform.position);
            //    float damageMultiplier = 1f - (distance / explosionRadius);
            //    float damage = damageAmount * damageMultiplier;
            //    damageable.TakeDamage(damage);
            //}

            //// 물리 효과
            //Rigidbody rb = hit.GetComponent<Rigidbody>();
            //if (rb != null)
            //{
            //    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            //}
        }

        // 폭발 후 오브젝트 제거
        Destroy(gameObject);
    }

    // 디버그용 - 폭발 범위 시각화 (Scene 뷰에서만 보임)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
