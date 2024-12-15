using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerXPManager : MonoBehaviour
{
    public Image xpForeground;                    // XP �� �̹���
    public TextMeshProUGUI levelText;             // ���� �ؽ�Ʈ
    public TextMeshProUGUI xpText;                // ���� ����ġ / �ִ� ����ġ �ؽ�Ʈ

    public int level = 1;                         // �÷��̾��� ����
    private float maxXP = 500;                    // �ִ� ����ġ
    private float currentXP = 0;                  // ���� ����ġ

    private PlayerHPManager playerHPManager;      // PlayerHPManager ����
    public LevelUpSelectionUI levelUpSelectionUI; // LevelUpSelectionUI ����

    void Start()
    {
        // ���� ������Ʈ���� PlayerHPManager ������Ʈ ��������
        playerHPManager = GetComponent<PlayerHPManager>();

        // LevelUpSelectionUI ���� Ȯ�� �� �ʱ�ȭ
        if (levelUpSelectionUI == null)
        {
            levelUpSelectionUI = FindObjectOfType<LevelUpSelectionUI>();
        }

        if (levelUpSelectionUI == null)
        {
            Debug.LogError("LevelUpSelectionUI�� ã�� �� �����ϴ�. levelUpSelectionUI�� null �����Դϴ�.");
        }
        else
        {
            Debug.Log("LevelUpSelectionUI�� ���������� ����Ǿ����ϴ�.");
            levelUpSelectionUI.HideLevelUpOptions(); // ���� ���� �� ����â �����
        }

        UpdateXPUI();
    }



    // ����ġ ȹ�� �޼���
    public void GainXP(float amount)
    {
        currentXP += amount;
        CheckLevelUp();
        UpdateXPUI();
    }

    // �ۼ�Ʈ�� ����ġ ȹ��
    public void GainXPByPercentage(float percentage)
    {
        currentXP += percentage * maxXP;
        CheckLevelUp();
        UpdateXPUI();
    }

    // ���� �� �޼���
    void LevelUp()
    {
        level++;
        float excessXP = currentXP - maxXP;
        maxXP = maxXP * (1 + 1.0f / Mathf.Sqrt(level)); // ������ ���� �ִ� XP ����
        currentXP = excessXP;

        UpdateXPUI();

        if (levelUpSelectionUI == null)
        {
            Debug.LogError("������ ����â�� ǥ���Ϸ� ������ levelUpSelectionUI�� null �����Դϴ�.");
            return; // ���� ������ ���� ���� �ߴ�
        }

        Debug.Log("������ ����â ǥ��");
        levelUpSelectionUI.ShowLevelUpOptions();
        //GameManager.Instance.TogglePause(); // ���� ����

    }




    // ������ ���� �Ϸ� �� ȣ��Ǵ� �޼��� (LevelUpSelectionUI ���ο��� ȣ��)
    public void OnLevelUpSelectionCompleted()
    {
        Debug.Log("������ ���� �Ϸ�. ���� �簳");
        GameManager.Instance.TogglePause(); // ���� �簳
        Debug.Log("Time.timeScale: " + Time.timeScale);
    }


    // ���� �� Ȯ�� �޼���
    void CheckLevelUp()
    {
        while (currentXP >= maxXP)
        {
            LevelUp();
        }
    }

    // ����ġ UI ������Ʈ �޼���
    void UpdateXPUI()
    {
        // XP �� ������Ʈ
        if (xpForeground != null)
        {
            xpForeground.fillAmount = currentXP / maxXP;
        }

        // ���� �ؽ�Ʈ ������Ʈ
        if (levelText != null)
        {
            levelText.text = "Lv " + level;
        }

        // ���� ����ġ / �ִ� ����ġ �ؽ�Ʈ ������Ʈ (���������� ǥ��)
        if (xpText != null)
        {
            xpText.text = $"{currentXP.ToString("F0")} / {maxXP.ToString("F0")}";
        }
    }

    void Update()
    {
        // �׽�Ʈ�� Ű �Է� ó��
        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXP(maxXP * 0.1f); // XP�� 10%�� �߰��� ȹ��
            Debug.Log("����ġ�� ȹ���߽��ϴ�.");
        }
    }

    // �������� ���� ���� ���� �޼��� �߰�
    public void IncreaseMaxHP()
    {
        Debug.Log("�ִ� HP ���� ����");
        if (playerHPManager != null)
        {
            float additionalHP = 200f;
            playerHPManager.IncreaseMaxHP(additionalHP);
            Debug.Log("�ִ� ü���� 200 �����߽��ϴ�.");
        }
        levelUpSelectionUI.HideLevelUpOptions(); // ���� �� UI �����
    }

    public void IncreaseAttackPower()
    {
        Debug.Log("���ݷ� ���� ����");
        levelUpSelectionUI.HideLevelUpOptions(); // ���� �� UI �����
    }

    public void IncreaseBulletCount()
    {
        Debug.Log("�߻� �ӵ� ���� ����");
        levelUpSelectionUI.HideLevelUpOptions(); // ���� �� UI �����
    }
}