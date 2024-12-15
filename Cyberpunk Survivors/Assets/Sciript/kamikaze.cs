using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class kamikaze : Monster
{
    public GameObject bombParticle;
    private MonsterSpawner monsterSpawner;
    GameObject player;
    //public float moveSpeed = 5.0f;
    //public float aroundSpeed = 20.0f;
    //public float stopDistance = 1.0f;
    private bool ground = false;
    //public float delay = 2.5f;
    Rigidbody rig;
    Animator anim;
    PlayerHPManager playerHP = null;
    private bool move = true;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Get the PlayerHPManager component when we find the player
            playerHP = player.GetComponent<PlayerHPManager>();
            if (playerHP == null)
            {
                Debug.LogWarning("PlayerHPManager not found ");
            }
        }
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        monsterSpawner = FindObjectOfType<MonsterSpawner>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(ground)
        {
            anim.SetBool("isGround",true);
        }
        if(player != null && ground && move)
        {
            float Distance = Vector3.Distance(transform.position, player.transform.position);
            if(Distance > stopDistance)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
                rig.MovePosition(newPosition);
                Vector3 newForward = Vector3.Lerp(transform.forward, direction, aroundSpeed*Time.deltaTime);
                transform.forward = newForward;
            }
        }
    }

    private void LateUpdate()
    {
        //Debug.Log(iHP);
        if (iHP <= 0)
        {
            MonsterDead();
            
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ground"))
        {
            ground = true;
        }
        if(other.collider.CompareTag("Player"))
        {
            playerHP = other.gameObject.GetComponent<PlayerHPManager>();
            if(playerHP != null)
            {
                playerHP.TakeDamage(100);
            }
            else
            {
                Debug.LogError("PlayerHPManager not found");
            }
            anim.SetTrigger("destroy");
            if (bombParticle != null)
            {
                int tmpRandom = Random.Range(1, 5);
                AudioManager.Instance.Play($"explosion_large_no_tail_0{tmpRandom}");
                Instantiate(bombParticle, transform.position, Quaternion.identity);
            }
            monsterSpawner.IncreaseKillCount();
            move = false;
            Destroy(gameObject,delay);
        }
    }
}
