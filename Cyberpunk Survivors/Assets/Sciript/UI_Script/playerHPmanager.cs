using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro�� ����ϱ� ���� �߰�

public class PlayerHPManager : MonoBehaviour
{
    public Image hpForeground;         // PlayerHPForeground UI �̹��� (ü�¹�)
    public TextMeshProUGUI hpText;     // ���� ü��/�ִ� ü���� ǥ���� �ؽ�Ʈ
    public int level = 1;              // �÷��̾��� ���� (�ʱ� �� 1)
    private float maxHP;               // �ִ� ü��
    private float currentHP;           // ���� ü��
    private float defense = 50;        // �⺻ ����
    private float hpRecoveryRate = 0.2f; // ü�� ȸ�� ���� (20%)
    public Text gameOverText;          // ���� ���� �ؽ�Ʈ�� UI�� ����
    private bool isDead = false;       // �÷��̾� ��� ���� Ȯ�� ����

    void Start()
    {
        SetMaxHP();             // ������ ���� �ִ� ü�� ����
        currentHP = maxHP;      // ���� �� �ִ� ü������ ����
        UpdateHPBar();          // HP�� �ʱ�ȭ

        // ���� ���� �ؽ�Ʈ �ʱ�ȭ
        if (gameOverText != null)
        {
            gameOverText.enabled = false;
        }
    }

    // �ִ� ü�� ���� �Լ� (������ ����)
    void SetMaxHP()
    {
        if (level == 1)
        {
            maxHP = 1000; // 1������ �� �ִ� ü���� 1000
        }
        else
        {
            maxHP = maxHP * (1 + (1.0f / level) * (1.0f / level)); // ���� ������ ���� ü�� ����
        }
    }

    // ü�� ȸ�� �Լ�
    void RecoverHP()
    {
        if (!isDead)
        {
            currentHP += maxHP * hpRecoveryRate;
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
            UpdateHPBar();
        }
    }

    // ������ �Դ� �Լ�
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        float finalDamage = damage - defense;
        if (finalDamage < 0) finalDamage = 0;

        currentHP -= finalDamage;
        if (currentHP < 0) currentHP = 0;

        UpdateHPBar();

        if (currentHP == 0) Die();
    }

    // �÷��̾� ��� ó�� �Լ�
    void Die()
    {
        isDead = true;
        Debug.Log("�÷��̾ ����߽��ϴ�!");

        if (gameOverText != null)
        {
            gameOverText.enabled = true;
            gameOverText.text = "GAME OVER";
        }

        Invoke("RestartGame", 2f);
    }

    // ���� ����� �Լ�
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // �ִ� HP ���� �޼���
    public void IncreaseMaxHP(float additionalHP)
    {
        maxHP += additionalHP;
        currentHP += additionalHP; // ���� ü�µ� �߰��� ��ŭ ȸ��
        UpdateHPBar();
        Debug.Log("�ִ� HP�� " + additionalHP + "��ŭ �����߽��ϴ�.");
    }

    // HP�ٿ� �ؽ�Ʈ ������Ʈ �Լ�
    void UpdateHPBar()
    {
        // ü�¹� ������Ʈ
        if (hpForeground != null)
        {
            hpForeground.fillAmount = currentHP / maxHP;
        }

        // ���� ü�� / �ִ� ü�� �ؽ�Ʈ ������Ʈ
        if (hpText != null)
        {
            hpText.text = $"{currentHP} / {maxHP}";
        }
    }

    // ���� �� �Լ�
    public void LevelUp()
    {
        level++;
        SetMaxHP();
        currentHP = maxHP;
        UpdateHPBar();
    }

    // �׽�Ʈ�� Ű �Է� ó��
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // H Ű�� ������ �� ü�� ȸ��
        {
            RecoverHP();
            Debug.Log("ü���� ȸ���Ǿ����ϴ�.");
        }

        if (Input.GetKeyDown(KeyCode.E)) // D Ű�� ������ �� ������ �Ա�
        {
            TakeDamage(100); // ���� ���� ���� ������ 100
            Debug.Log("�������� �Ծ����ϴ�.");
        }

        if (Input.GetKeyDown(KeyCode.L)) // L Ű�� ������ �� ���� ��
        {
            LevelUp();
            Debug.Log("������ �߽��ϴ�. ���� ����: " + level + ", �ִ� ü��: " + maxHP);
        }
    }
}
