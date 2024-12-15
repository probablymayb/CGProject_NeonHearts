using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerXPManager : MonoBehaviour
{
    public Image xpForeground;                    // XP 바 이미지
    public TextMeshProUGUI levelText;             // 레벨 텍스트
    public TextMeshProUGUI xpText;                // 현재 경험치 / 최대 경험치 텍스트

    public int level = 1;                         // 플레이어의 레벨
    private float maxXP = 500;                    // 최대 경험치
    private float currentXP = 0;                  // 현재 경험치

    private PlayerHPManager playerHPManager;      // PlayerHPManager 연결
    public LevelUpSelectionUI levelUpSelectionUI; // LevelUpSelectionUI 연결

    void Start()
    {
        // 같은 오브젝트에서 PlayerHPManager 컴포넌트 가져오기
        playerHPManager = GetComponent<PlayerHPManager>();

        // LevelUpSelectionUI 연결 확인 및 초기화
        if (levelUpSelectionUI == null)
        {
            levelUpSelectionUI = FindObjectOfType<LevelUpSelectionUI>();
        }

        if (levelUpSelectionUI == null)
        {
            Debug.LogError("LevelUpSelectionUI를 찾을 수 없습니다. levelUpSelectionUI가 null 상태입니다.");
        }
        else
        {
            Debug.Log("LevelUpSelectionUI가 정상적으로 연결되었습니다.");
            levelUpSelectionUI.HideLevelUpOptions(); // 게임 시작 시 선택창 숨기기
        }

        UpdateXPUI();
    }



    // 경험치 획득 메서드
    public void GainXP(float amount)
    {
        currentXP += amount;
        CheckLevelUp();
        UpdateXPUI();
    }

    // 퍼센트로 경험치 획득
    public void GainXPByPercentage(float percentage)
    {
        currentXP += percentage * maxXP;
        CheckLevelUp();
        UpdateXPUI();
    }

    // 레벨 업 메서드
    void LevelUp()
    {
        level++;
        float excessXP = currentXP - maxXP;
        maxXP = maxXP * (1 + 1.0f / Mathf.Sqrt(level)); // 레벨에 따라 최대 XP 증가
        currentXP = excessXP;

        UpdateXPUI();

        if (levelUpSelectionUI == null)
        {
            Debug.LogError("레벨업 선택창을 표시하려 했으나 levelUpSelectionUI가 null 상태입니다.");
            return; // 에러 방지를 위해 실행 중단
        }

        Debug.Log("레벨업 선택창 표시");
        levelUpSelectionUI.ShowLevelUpOptions();
        //GameManager.Instance.TogglePause(); // 게임 멈춤

    }




    // 레벨업 선택 완료 시 호출되는 메서드 (LevelUpSelectionUI 내부에서 호출)
    public void OnLevelUpSelectionCompleted()
    {
        Debug.Log("레벨업 선택 완료. 게임 재개");
        GameManager.Instance.TogglePause(); // 게임 재개
        Debug.Log("Time.timeScale: " + Time.timeScale);
    }


    // 레벨 업 확인 메서드
    void CheckLevelUp()
    {
        while (currentXP >= maxXP)
        {
            LevelUp();
        }
    }

    // 경험치 UI 업데이트 메서드
    void UpdateXPUI()
    {
        // XP 바 업데이트
        if (xpForeground != null)
        {
            xpForeground.fillAmount = currentXP / maxXP;
        }

        // 레벨 텍스트 업데이트
        if (levelText != null)
        {
            levelText.text = "Lv " + level;
        }

        // 현재 경험치 / 최대 경험치 텍스트 업데이트 (정수형으로 표시)
        if (xpText != null)
        {
            xpText.text = $"{currentXP.ToString("F0")} / {maxXP.ToString("F0")}";
        }
    }

    void Update()
    {
        // 테스트용 키 입력 처리
        if (Input.GetKeyDown(KeyCode.X))
        {
            GainXP(maxXP * 0.1f); // XP의 10%를 추가로 획득
            Debug.Log("경험치를 획득했습니다.");
        }
    }

    // 선택지에 따른 스탯 증가 메서드 추가
    public void IncreaseMaxHP()
    {
        Debug.Log("최대 HP 증가 선택");
        if (playerHPManager != null)
        {
            float additionalHP = 200f;
            playerHPManager.IncreaseMaxHP(additionalHP);
            Debug.Log("최대 체력이 200 증가했습니다.");
        }
        levelUpSelectionUI.HideLevelUpOptions(); // 선택 후 UI 숨기기
    }

    public void IncreaseAttackPower()
    {
        Debug.Log("공격력 증가 선택");
        levelUpSelectionUI.HideLevelUpOptions(); // 선택 후 UI 숨기기
    }

    public void IncreaseBulletCount()
    {
        Debug.Log("발사 속도 증가 선택");
        levelUpSelectionUI.HideLevelUpOptions(); // 선택 후 UI 숨기기
    }
}