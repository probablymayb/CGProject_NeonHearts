using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro를 사용하기 위해 추가

public class PlayerHPManager : MonoBehaviour
{
    public Image hpForeground;         // PlayerHPForeground UI 이미지 (체력바)
    public TextMeshProUGUI hpText;     // 현재 체력/최대 체력을 표시할 텍스트
    public int level = 1;              // 플레이어의 레벨 (초기 값 1)
    private float maxHP;               // 최대 체력
    private float currentHP;           // 현재 체력
    private float defense = 50;        // 기본 방어력
    private float hpRecoveryRate = 0.2f; // 체력 회복 비율 (20%)
    public Text gameOverText;          // 게임 오버 텍스트를 UI에 연결
    private bool isDead = false;       // 플레이어 사망 여부 확인 변수

    void Start()
    {
        SetMaxHP();             // 레벨에 따라 최대 체력 설정
        currentHP = maxHP;      // 시작 시 최대 체력으로 설정
        UpdateHPBar();          // HP바 초기화

        // 게임 오버 텍스트 초기화
        if (gameOverText != null)
        {
            gameOverText.enabled = false;
        }
    }

    // 최대 체력 설정 함수 (레벨에 따라)
    void SetMaxHP()
    {
        if (level == 1)
        {
            maxHP = 1000; // 1레벨일 때 최대 체력은 1000
        }
        else
        {
            maxHP = maxHP * (1 + (1.0f / level) * (1.0f / level)); // 레벨 증가에 따라 체력 증가
        }
    }

    // 체력 회복 함수
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

    // 데미지 입는 함수
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

    // 플레이어 사망 처리 함수
    void Die()
    {
        isDead = true;
        Debug.Log("플레이어가 사망했습니다!");

        if (gameOverText != null)
        {
            gameOverText.enabled = true;
            gameOverText.text = "GAME OVER";
        }

        Invoke("RestartGame", 2f);
    }

    // 게임 재시작 함수
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 최대 HP 증가 메서드
    public void IncreaseMaxHP(float additionalHP)
    {
        maxHP += additionalHP;
        currentHP += additionalHP; // 현재 체력도 추가된 만큼 회복
        UpdateHPBar();
        Debug.Log("최대 HP가 " + additionalHP + "만큼 증가했습니다.");
    }

    // HP바와 텍스트 업데이트 함수
    void UpdateHPBar()
    {
        // 체력바 업데이트
        if (hpForeground != null)
        {
            hpForeground.fillAmount = currentHP / maxHP;
        }

        // 현재 체력 / 최대 체력 텍스트 업데이트
        if (hpText != null)
        {
            hpText.text = $"{currentHP} / {maxHP}";
        }
    }

    // 레벨 업 함수
    public void LevelUp()
    {
        level++;
        SetMaxHP();
        currentHP = maxHP;
        UpdateHPBar();
    }

    // 테스트용 키 입력 처리
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // H 키를 눌렀을 때 체력 회복
        {
            RecoverHP();
            Debug.Log("체력이 회복되었습니다.");
        }

        if (Input.GetKeyDown(KeyCode.E)) // D 키를 눌렀을 때 데미지 입기
        {
            TakeDamage(100); // 데모를 위해 고정 데미지 100
            Debug.Log("데미지를 입었습니다.");
        }

        if (Input.GetKeyDown(KeyCode.L)) // L 키를 눌렀을 때 레벨 업
        {
            LevelUp();
            Debug.Log("레벨업 했습니다. 현재 레벨: " + level + ", 최대 체력: " + maxHP);
        }
    }
}
