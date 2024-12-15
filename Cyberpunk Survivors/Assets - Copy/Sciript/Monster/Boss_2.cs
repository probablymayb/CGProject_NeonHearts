using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Boss_2 : MonoBehaviour
{
    GameObject player;
    public float moveSpeed = 1.5f;
    public float aroundSpeed = 20.0f;
    public float stopDistance = 1.0f;
    private bool ground = false;
    public float delay = 2.5f;
    Rigidbody rig;
    Animator anim;

    PlayerHPManager playerHP = null;
    public int miniCount = 10;
    public float miniinterval = 2.0f;
    private bool move = true;
    private float spreadAngle = 30f;
    private int bombCount = 3;
    private float initialSpeed = 15f;
    private float launchAngle = 45f;
    public GameObject bombPrefab = null;
    private float bomb_interval = 5f;
    private float next_bomb = 0f;
    static public bool boss_1_dead = false;

    // Start is called before the first frame update
    void Start()
    {
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
        next_bomb = Time.time + bomb_interval;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
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
        if(Time.time >= next_bomb)
        {
            CastBombSkill();
            next_bomb = Time.time + bomb_interval;
        }
    }
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Ground"))
        {
            ground = true;
            
        }
        if(other.collider.CompareTag("Player"))
        {
        

            if (other.collider.CompareTag("Player"))
            {
                boss_1_dead = true;
                playerHP = other.gameObject.GetComponent<PlayerHPManager>();
                if (playerHP != null)
                {
                    playerHP.TakeDamage(100);
                }
                else
                {
                    Debug.LogError("PlayerHPManager not found");
                }
                move = false;
                anim.SetTrigger("destroy");
                //Destroy(gameObject, delay);
            }
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
        Vector3 startPos = this.transform.position + Vector3.up * 1.5f;
        float randomSpread = Random.Range(-spreadAngle, spreadAngle);
        Vector3 direction = Quaternion.Euler(0, randomSpread, 0) * this.transform.forward;

        GameObject bomb = Instantiate(bombPrefab, startPos, Quaternion.identity);
        Rigidbody rb = bomb.GetComponent<Rigidbody>();


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
