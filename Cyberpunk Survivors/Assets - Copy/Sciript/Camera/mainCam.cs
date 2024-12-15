using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCam : MonoBehaviour
{
    public Transform player_cam; // 플레이어 Transform
    public Transform boss;   // 보스 Transform
    public float bossViewDuration = 3f; // 보스를 비추는 시간
    private Vector3 initialPosition; // 카메라의 원래 위치

    GameObject player;
    Vector3 offset;
    public float moveSpeed = 5.0f;
    void Start()
    {
        initialPosition = transform.position;
        AudioManager.Instance.Play("main bgm");
        AudioManager.Instance.Play("ambience_rain");

        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, moveSpeed*Time.deltaTime);

    }

    public void FocusOnBoss()
    {
        StartCoroutine(MoveCameraToBoss());
    }

    IEnumerator MoveCameraToBoss()
    {
        // 1. 보스 위치로 이동
        Vector3 bossPosition = new Vector3(boss.position.x, transform.position.y, boss.position.z);
        while (Vector3.Distance(transform.position, bossPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, bossPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 2. 보스를 일정 시간 동안 비추기
        yield return new WaitForSeconds(bossViewDuration);

        // 3. 플레이어 위치로 복귀
        Vector3 playerPosition = new Vector3(player_cam.position.x, transform.position.y, player_cam.position.z);
        while (Vector3.Distance(transform.position, playerPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, playerPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}