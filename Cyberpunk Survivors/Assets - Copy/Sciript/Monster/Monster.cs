using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    GameObject player;
    public float moveSpeed = 5.0f;
    public float aroundSpeed = 20.0f;
    public float stopDistance = 1.0f;
    private bool ground = false;
    public float delay = 2.5f;
    Rigidbody rig;
    Animator anim;

    PlayerHPManager playerHP = null;
    PlayerXPManager playerXP = null;

    private MonsterSpawner monsterSpawner;
    private bool move = true;

    //HP ���� ������
    protected int iHP = 5;
    private bool isDamaging = false;

    //�Ѿ� ��ȣ�ۿ�
    [SerializeField]
    protected GameObject DeadEffect = null;
    protected bool isDeadEffectSpawned = false;

    public void Start()
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

            playerXP = player.GetComponent<PlayerXPManager>();
            if (playerXP == null)
            {
                Debug.LogWarning("PlayerXPManager not found ");
            }
        }

        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        monsterSpawner = FindObjectOfType<MonsterSpawner>();
        
    }
    // Update is called once per frame
    
    public void FixedUpdate()
    {
        if (ground)
        {
            anim.SetBool("isGround", true);
        }
        if (player != null && ground && move)
        {
            float Distance = Vector3.Distance(transform.position, player.transform.position);
            if (Distance > stopDistance)
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
        if (iHP <= 0)
        {
            if (move)
            {
                playerXP.GainXPByPercentage(0.5f);

            }
            MonsterDead();
        }
    }
    public void TakeDmg(int dmg)
    {
        iHP -= dmg;
    }

    public virtual void MonsterDead()
    {

        StartCoroutine(SpawnDeadEffect());
        anim.SetTrigger("destroy");
        monsterSpawner.IncreaseKillCount();

        // ScoreManager와 연동
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.IncreaseKillScore();
        }

        move = false;

        Destroy(gameObject, delay);

    }

    protected IEnumerator SpawnDeadEffect()
    {
        yield return new WaitForSeconds(delay - 0.2f);

        if (!isDeadEffectSpawned)
        {
            Debug.LogWarning("Dead Effect spawned");
            int tmpRandom = Random.Range(1, 5);
            AudioManager.Instance.Play($"explosion_small_0{tmpRandom}");
            Instantiate(DeadEffect, transform.position, Quaternion.identity);
            isDeadEffectSpawned = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            ground = true;
        }
        if (other.collider.CompareTag("Player"))
        {
            playerHP = other.gameObject.GetComponent<PlayerHPManager>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(100);
            }
            else
            {
                Debug.LogError("PlayerHPManager not found");
            }
           // MonsterDead();
        }

    }

    private void OnCollisionExit(Collision other)
    {
        // 플레이어와 충돌이 끝났을 때 데미지 중단
        if (other.collider.CompareTag("Player"))
        {
            StopAllCoroutines(); // 모든 코루틴 중지
            isDamaging = false; // 데미지 중 상태 초기화
        }
    }

    private IEnumerator DamagePlayerOverTime(PlayerHPManager playerHP)
    {
        isDamaging = true; // 데미지 중 상태로 설정
        while (true)
        {
            playerHP.TakeDamage(50); // 데미지 입히기
            yield return new WaitForSeconds(5f); // 5초 대기
        }
    }

     
}
