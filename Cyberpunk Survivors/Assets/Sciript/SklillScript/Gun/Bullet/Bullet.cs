using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float fBulletSpeed = 1f;
    [SerializeField]
    float fKillTime = 3f;
    [SerializeField]
    GameObject BulletShotEffect = null;

    [SerializeField]
    private int iBulletDmg = 1;

    void Start()
    {
        Destroy(gameObject, fKillTime);
    }

    void Update()
    {
        transform.position += transform.forward * fBulletSpeed * Time.deltaTime;
    }

    public void SetDmg(int dmg)
    {
        iBulletDmg = dmg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 hitPosition = contact.point;

        Quaternion hitRotation = Quaternion.LookRotation(contact.normal);

        if (BulletShotEffect != null)
        {
            GameObject effect = Instantiate(BulletShotEffect, hitPosition, hitRotation);
            Destroy(effect, 2f);
        }

        if (collision.gameObject.CompareTag("monster"))
        {
            Monster monsterComponent = collision.gameObject.GetComponent<Monster>();
            if (monsterComponent != null)
            {
                monsterComponent.TakeDmg(iBulletDmg);
                AudioManager.Instance.Play("hit sound");
                Debug.Log($"Hit Monster Type: {monsterComponent.GetType().Name}");
            }

        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
        }
        else
        {
            Debug.Log("Hit: " + collision.gameObject.name);
        }

        Instantiate(BulletShotEffect, transform.position, Quaternion.identity);

        Destroy(gameObject, 0.5f);
    }
}