using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Monster
{
    public GameObject alien;
    public GameObject jangpoongPrefab;
    public float jTime = 5f;
    private float nextJtime = 0f;
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
    public float jumpForce = 10f;
    public float jumpCoolDown = 5f;
    private bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHP = player.GetComponent<PlayerHPManager>();
            if (playerHP == null)
            {
                Debug.LogWarning("PlayerHPManager not found ");
            }
        }

        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        StartCoroutine(miniMonsterSpawner());
        nextJtime = Time.time + jTime;
        InvokeRepeating("JumpToPlayer", 13f, jumpCoolDown);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null && ground && move)
        {
            float Distance = Vector3.Distance(transform.position, player.transform.position);
            if (Distance > stopDistance)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
                rig.MovePosition(newPosition);
                Vector3 newForward = Vector3.Lerp(transform.forward, direction, aroundSpeed * Time.deltaTime);
                transform.forward = newForward;
            }
        }
        if (Time.time >= nextJtime && !isJumping)
        {
            StartCoroutine(ShootJangpoongWithAnimation());
            nextJtime = Time.time + jTime;
        }
        BoxCollider collider = GetComponent<BoxCollider>();
        Vector3 playerPosition = player.transform.position;
        float adjustedY = playerPosition.y + collider.size.y/2.0f;
        collider.center = new Vector3(collider.center.x, adjustedY, collider.center.z);
    }

    IEnumerator ShootJangpoongWithAnimation()
    {
        move = false; // 이동을 멈추기
        anim.SetBool("isWalking", false); // 걷기 애니메이션 멈추기

        anim.SetTrigger("shootJangpoong"); // 장풍을 쏘는 애니메이션 트리거

        // 애니메이션이 끝날 때까지 기다리기
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        shootJangpoong();
        StartCoroutine(resumeWalkingAfterJangpoong());
    }

    private void shootJangpoong()
    {
        if (jangpoongPrefab == null)
        {
            Debug.Log("장풍프리팹없음");
        }
        if (jangpoongPrefab != null)
        {
            Vector3 jPos = transform.position + new Vector3(0, 1, 0);
            GameObject jangpoong = Instantiate(jangpoongPrefab, jPos, transform.rotation);
            Rigidbody jpRigidbody = jangpoong.GetComponent<Rigidbody>();
            if (jpRigidbody != null)
            {
                jpRigidbody.AddForce(transform.forward * 15f, ForceMode.VelocityChange);
            }
            Debug.Log("장풍 생성됨 : " + jangpoong.name);
            StartCoroutine(resumeWalkingAfterJangpoong());
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
            anim.SetTrigger("destroy");
            //Destroy(gameObject, delay);
            move = false;
        }
    }

    IEnumerator miniMonsterSpawner()
    {
        yield return new WaitUntil(() => ground);
        for (int i = 0; i < miniCount; i++)
        {
            Vector3 miniMonPos = transform.position + new Vector3(-0.5f, 0, 0.5f);
            Instantiate(alien, miniMonPos, Quaternion.identity);
            yield return new WaitForSeconds(miniinterval);
        }
    }

    IEnumerator resumeWalkingAfterJangpoong()
    {
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("isWalking", true);
        move = true;
    }

    private void JumpToPlayer()
    {
        if (player != null && ground && !isJumping)
        {
            isJumping = true;
            move = false;
            anim.SetBool("jump",true);
            float jumpAnimationLength = anim.GetCurrentAnimatorStateInfo(0).length;
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                Debug.LogWarning("jump전환 안됨.");
            }
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.forward = direction;
            Vector3 jumpTarget = transform.position + direction * 2f;

            //점프 효과를 위해 Rigidbody에 힘을 추가
            // 점프 효과를 위해 Rigidbody에 힘을 추가
            Vector3 jumpDirection = (jumpTarget - transform.position).normalized * jumpForce;
            rig.AddForce(new Vector3(jumpDirection.x, jumpForce, jumpDirection.z), ForceMode.Impulse);

            StartCoroutine(ResumeWalkingAfterJump(jumpAnimationLength));
        }
    }

    IEnumerator ResumeWalkingAfterJump(float animationLenght)
    {
        yield return new WaitForSeconds(animationLenght);
        isJumping = false;
        move = true;
        anim.SetBool("jump", false);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.forward = direction;
    }

    public override void MonsterDead()
    {
        Debug.Log("Boss is defeated! MonsterDead() called."); // 로그 추가
        //Camera.main.GetComponent<mainCam>().FocusOnBoss();
        base.MonsterDead(); // 부모 클래스의 동작 호출
        AudioManager.Instance.Stop("main bgm");
        StartCoroutine(TransitionToGameOver()); // 추가 동작: 종료 화면으로 전환
    }

    private IEnumerator TransitionToGameOver()
    {
        Debug.Log("Transition to GameOverScene started."); // 로그 추가
        yield return new WaitForSeconds(2.0f); // 보스 사망 애니메이션 후 대기
        SceneManager.LoadScene("GameOverScene"); // 종료 화면으로 전환
    }
}
