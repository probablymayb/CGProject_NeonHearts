using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public float damageAmount = 50f;
    public GameObject explosionEffectPrefab;
    private bool hasExploded = false;
    private float timer = 0f;  // Ÿ�̸� �߰�
    private float explosionDelay = 1f;  // ���� ���� �ð�

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
        // ���� ����Ʈ ����
        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            // ��ƼŬ �ý����� �ִٸ� �ڵ����� ���ŵǵ��� ����
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

        // ���� ���� �� ������Ʈ ���� �� ������ ó��
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            //IDamageable damageable = hit.GetComponent<IDamageable>();
            //if (damageable != null)
            //{
            //    // �Ÿ��� ���� ������ ����
            //    float distance = Vector3.Distance(transform.position, hit.transform.position);
            //    float damageMultiplier = 1f - (distance / explosionRadius);
            //    float damage = damageAmount * damageMultiplier;
            //    damageable.TakeDamage(damage);
            //}

            //// ���� ȿ��
            //Rigidbody rb = hit.GetComponent<Rigidbody>();
            //if (rb != null)
            //{
            //    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            //}
        }

        // ���� �� ������Ʈ ����
        Destroy(gameObject);
    }

    // ����׿� - ���� ���� �ð�ȭ (Scene �信���� ����)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
