using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGun : MonoBehaviour
{
    GameObject player = null;

    [SerializeField]
    GameObject bullet = null;

    [SerializeField]
    float fCoolTime = 0f;

    [SerializeField]
    Transform rootBone = null;

    float fTimer = 0f;

    bool isUpgraded = false;

    [SerializeField]
    private Vector3 positionOffset = Vector3.zero;
    [SerializeField]
    private Vector3 rotationOffset = Vector3.zero;


    [SerializeField]
    private int iGunDamage = 1; // 총 기본 데미지
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //Transform leftHand = player.transform.Find("LeftHand");

            //if (leftHand != null)
            {
                transform.SetParent(rootBone);
                transform.localPosition = positionOffset;
                transform.localRotation = Quaternion.Euler(rotationOffset);
            }
            //else
            //{
            //    Debug.LogWarning("LeftHand bone not found.");
            //}
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isUpgraded = !isUpgraded;
        }
        fTimer += Time.deltaTime;
        if (fTimer > fCoolTime )
        {
            GenerateBullet();
            fTimer = 0f;
        }
    }

    int getGunDamage()
    {
        return iGunDamage;
    }

    public void UpgradeDmg(int dmg)
    {
        iGunDamage = dmg;
    }

    public void UpgradeBullet()
    {
        isUpgraded = true;
    }

    void GenerateBullet()
    {
        Vector3 spawnPos = this.transform.position + player.transform.forward * 2f;

        if (!isUpgraded)
        {
            // Instantiate<T>를 사용하여 바로 Bullet 컴포넌트 받기
            AudioManager.Instance.Play("gun shoot");
            Bullet newBullet = Instantiate<Bullet>(bullet.GetComponent<Bullet>(), spawnPos, player.transform.rotation);
            newBullet.SetDmg(iGunDamage);
        }
        else
        {
            AudioManager.Instance.Play("3way gun shot");
            Bullet centerBullet = Instantiate<Bullet>(bullet.GetComponent<Bullet>(), spawnPos, player.transform.rotation);
            centerBullet.SetDmg(iGunDamage);

            Quaternion leftRotation = player.transform.rotation * Quaternion.Euler(0, -30, 0);
            Quaternion rightRotation = player.transform.rotation * Quaternion.Euler(0, 30, 0);

            Bullet leftBullet = Instantiate<Bullet>(bullet.GetComponent<Bullet>(), spawnPos, leftRotation);
            leftBullet.SetDmg(iGunDamage);

            Bullet rightBullet = Instantiate<Bullet>(bullet.GetComponent<Bullet>(), spawnPos, rightRotation);
            rightBullet.SetDmg(iGunDamage);
        }


        Debug.LogWarning("Bullet Generated.");
    }
}

