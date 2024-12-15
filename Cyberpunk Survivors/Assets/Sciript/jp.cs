using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jp : MonoBehaviour
{
    public float damage = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision Detected with: "+ other.gameObject.name);
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerHPManager playerHP = other.gameObject.GetComponent<PlayerHPManager>();
            if(playerHP != null)
            {
                playerHP.TakeDamage(damage);
                Debug.Log("플레이어가 장풍에 맞음 데미지 :" + damage);
            }
            Destroy(gameObject);
        }
        if(other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("jangpoong hit the ground");
            Destroy(gameObject);
        }    
    }
}
