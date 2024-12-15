using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Missile : MonoBehaviour
{
    private Vector3 targetPosition;
    public float targetingRadius;
    public float explosionRadius;
    public float speed = 20f;
    public GameObject[] explosionEffectPrefabs;

    public event System.Action onExplode;

    public void Initialize(Vector3 target, float radius)
    {
        targetPosition = target;
        targetingRadius = radius;
        transform.LookAt(targetPosition);
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget < 0.1f)
        {
            Explode();
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    void Explode()
    {
        onExplode?.Invoke(); // 이벤트 발생

        if (explosionEffectPrefabs != null && explosionEffectPrefabs.Length > 0)
        {
            int rndno = Random.Range(1, 5);

            AudioManager.Instance.PlayOnceAtATime($"explosion_large_no_tail_0{rndno}");

            int rnd = Random.Range(0, explosionEffectPrefabs.Length);
            GameObject effect = Instantiate(explosionEffectPrefabs[rnd], transform.position, Quaternion.identity);
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(effect, ps.main.duration);
            }
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            // 충돌 처리 로직
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Trigger 충돌 시 호출되는 함수
        if (other.CompareTag("monster"))
        {
            Monster monsterComponent = other.GetComponent<Monster>();
            if (monsterComponent != null)
            {
                monsterComponent.TakeDmg(1000);
                AudioManager.Instance.Play("hit sound");
                Debug.Log($"Hit Monster Type: {monsterComponent.GetType().Name}");
            }
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
        }
        else
        {
            Debug.Log("Hit: " + other.gameObject.name);
        }

        //Instantiate(BulletShotEffect, transform.position, Quaternion.identity);
        //Destroy(gameObject, 0.5f);
    }
}