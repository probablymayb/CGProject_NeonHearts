using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float vAxis;
    float hAxis;
    public float moveSpeed = 10f;
    public float aroundSpeed = 20f;
    Vector3 move = Vector3.zero;
    Rigidbody rig;
    Animator anim;
    public Transform gun;
    public GameObject wich;

    private bool canPlayFootstep = true;
    private float footstepInterval = 0.4f;


    //skill test
    public GameObject bombPrefab = null;
    private float initialSpeed = 15f;
    private float launchAngle = 45f;
    private float spreadAngle = 30f;
    private int bombCount = 3;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //바로 Vector3 의 x, z좌표에다 인풋을 받는방식 , 그리고 그 Vector3 를 포지션이동과 forward이동에 적용

        /*move.x = Input.GetAxisRaw("Horizontal");
        move.z = Input.GetAxisRaw("Vertical");
        
        transform.position += move.normalized * moveSpeed * Time.deltaTime;
        transform.forward = move;*/


        // 선언되어있는 float 변수에 인풋값을 각각 받고 그 변수를 통해 Vector3의 값을 변경시켜주고 그 값을 포지션이동과 forward이동에 적용
    
        vAxis = Input.GetAxisRaw("Vertical");
        hAxis = Input.GetAxisRaw("Horizontal");
        move = new Vector3(hAxis,0,vAxis).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;
        transform.forward += move * aroundSpeed * Time.deltaTime;
        // Jump입력시 ground값이 true 일 때 점프 한번 점프후 점프를 false값으로 변경
        
        if(hAxis != 0 || vAxis != 0)
        {
            anim.SetBool("isMove",true);
            //AudioManager.Instance.PlayOnceAtATime("footstep_water_run_01");

            if (canPlayFootstep)
            {
                StartCoroutine(PlayFootstepSound());
            }
        }
        if (hAxis == 0 && vAxis == 0)
        {
            anim.SetBool("isMove",false);

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CastBombSkill();
        }
        //Shoot();
    }

    void Shoot()
    {
        Instantiate(wich, gun.position, Quaternion.identity);
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

    IEnumerator PlayFootstepSound()
    {
        canPlayFootstep = false;

    
        int randomSound1 = Random.Range(1, 9);
        AudioManager.Instance.Play($"footstep_water_run_0{randomSound1}");
       

        yield return new WaitForSeconds(footstepInterval);
        AudioManager.Instance.Play("footstep_water_run_02");

        yield return new WaitForSeconds(footstepInterval);
        canPlayFootstep = true;
    }
}