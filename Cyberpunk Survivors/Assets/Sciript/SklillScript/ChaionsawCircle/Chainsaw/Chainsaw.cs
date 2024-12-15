using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chainsaw : MonoBehaviour
{

    public Transform target;
    public float orbitSpeed;
    Vector3 offset;

    [SerializeField]
    private GameObject ChainsawShotEffect = null;

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("monster"))
        {
            if (collision.gameObject.TryGetComponent<Monster>(out Monster monsterComponent))
            {
                monsterComponent.TakeDmg(10);
            }

            if (collision.gameObject.TryGetComponent<kamikaze>(out kamikaze kamikazeComponent))
            {
                kamikazeComponent.TakeDmg(10);

            }
            Debug.Log("CHAINSAW!!!!!!!!!!!!!!!");
            AudioManager.Instance.PlayOnceAtATime("Chainsaw");


            Instantiate(ChainsawShotEffect, transform.position, Quaternion.identity);

        }
    }

}
