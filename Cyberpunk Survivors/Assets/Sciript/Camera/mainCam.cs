using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCam : MonoBehaviour
{
    public Transform player_cam; // �÷��̾� Transform
    public Transform boss;   // ���� Transform
    public float bossViewDuration = 3f; // ������ ���ߴ� �ð�
    private Vector3 initialPosition; // ī�޶��� ���� ��ġ

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
        // 1. ���� ��ġ�� �̵�
        Vector3 bossPosition = new Vector3(boss.position.x, transform.position.y, boss.position.z);
        while (Vector3.Distance(transform.position, bossPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, bossPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 2. ������ ���� �ð� ���� ���߱�
        yield return new WaitForSeconds(bossViewDuration);

        // 3. �÷��̾� ��ġ�� ����
        Vector3 playerPosition = new Vector3(player_cam.position.x, transform.position.y, player_cam.position.z);
        while (Vector3.Distance(transform.position, playerPosition) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, playerPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}