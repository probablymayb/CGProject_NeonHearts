using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject selectionPanel;
    [SerializeField] private Button increaseMaxHPButton;
    [SerializeField] private Button increaseAttackPowerButton;
    [SerializeField] private Button increaseBulletCountButton;
    [SerializeField] private Button increaseChainsawButton;
    [SerializeField] private Button BombardButton;


    public PlayerXPManager playerXPManager;
    private ChainsawCircle chainsawCircle;
    private MissileBombard missileBombard;

    //스킬 업데이트용 추가
    private NormalGun gunScript;

    void Awake()
    {
        // selectionPanel이 제대로 설정되었는지 확인하고 초기화
        if (selectionPanel == null)
        {
            Debug.LogError("selectionPanel이 null 상태입니다. UI를 확인하세요.");
        }
        else
        {
            selectionPanel.SetActive(false); // 기본적으로 비활성화
            Debug.Log("selectionPanel 초기화 완료.");
        }

        // 버튼 확인 및 초기화
        if (increaseMaxHPButton != null)
            increaseMaxHPButton.onClick.AddListener(OnIncreaseMaxHPSelected);
        else
            Debug.LogError("IncreaseMaxHPButton이 연결되지 않았습니다!");

        if (increaseAttackPowerButton != null)
            increaseAttackPowerButton.onClick.AddListener(OnIncreaseAttackPowerSelected);
        else
            Debug.LogError("IncreaseAttackPowerButton이 연결되지 않았습니다!");

        if (increaseBulletCountButton != null)
            increaseBulletCountButton.onClick.AddListener(OnIncreaseBulletCountSelected);
        else
            Debug.LogError("IncreaseBulletCountButton이 연결되지 않았습니다!");

        if (increaseChainsawButton != null)
            increaseChainsawButton.onClick.AddListener(OnIncreaseChainsawSelected);
        else
            Debug.LogError("OnIncreaseChainsawSelected 연결되지 않았습니다!");

        if (BombardButton != null)
            BombardButton.onClick.AddListener(OnBombardSelected);
        else
            Debug.LogError("BombardButton 연결되지 않았습니다!");
    }


    void Start()
    {

        gunScript = GetComponentInParent<Player>().GetComponentInChildren<NormalGun>();
        if (gunScript == null)
        {
            Debug.LogError("Gun 스크립트를 찾을 수 없습니다!");
        }
        chainsawCircle = GameObject.FindGameObjectWithTag("ChainsawCircle").GetComponent<ChainsawCircle>();
        if (chainsawCircle == null)
        {
            Debug.LogError("chainsawCircle 연결되지 않았습니다.");
        }

        missileBombard = GameObject.FindGameObjectWithTag("MissileBombard").GetComponent<MissileBombard>();
        if (chainsawCircle == null)
        {
            Debug.LogError("chainsawCircle 연결되지 않았습니다.");
        }

        if (playerXPManager == null)
        {
            Debug.LogError("PlayerXPManager가 연결되지 않았습니다.");
        }
        if (increaseMaxHPButton == null)
            Debug.LogError("IncreaseMaxHPButton이 연결되지 않았습니다!");
        else
            Debug.Log("IncreaseMaxHPButton 연결됨.");

        if (increaseAttackPowerButton == null)
            Debug.LogError("IncreaseAttackPowerButton이 연결되지 않았습니다!");
        else
            Debug.Log("IncreaseAttackPowerButton 연결됨.");

        if (increaseBulletCountButton == null)
            Debug.LogError("IncreaseBulletCountButton이 연결되지 않았습니다!");
        else
            Debug.Log("IncreaseBulletCountButton 연결됨.");
        
        if (increaseChainsawButton == null)
            Debug.LogError("increaseChainsawButton 연결되지 않았습니다!");
        else
            Debug.Log("increaseChainsawButton 연결됨.");
        
        if (BombardButton == null)
            Debug.LogError("BombardButton 연결되지 않았습니다!");
        else
            Debug.Log("BombardButton 연결됨.");

        // 버튼에 클릭 이벤트 할당
        if (increaseMaxHPButton != null)
            increaseMaxHPButton.onClick.AddListener(OnIncreaseMaxHPSelected);
        if (increaseAttackPowerButton != null)
            increaseAttackPowerButton.onClick.AddListener(OnIncreaseAttackPowerSelected);
        if (increaseBulletCountButton != null)
            increaseBulletCountButton.onClick.AddListener(OnIncreaseBulletCountSelected);
        if (increaseChainsawButton != null)
            increaseChainsawButton.onClick.AddListener(OnIncreaseChainsawSelected);
        
        if (BombardButton != null)
            BombardButton.onClick.AddListener(OnBombardSelected);
    }

    public void ShowLevelUpOptions()
    {
        if (selectionPanel != null)
        {
            AudioManager.Instance.Play("level up");
            selectionPanel.SetActive(true); // 선택창 활성화
            Debug.Log("레벨업 선택창 표시");
        }
        else
        {
            Debug.LogError("레벨업 선택창을 표시하려 했으나 selectionPanel이 null 상태입니다.");
        }

        // Time.timeScale이 0일 경우 버튼 입력 활성화
        EventSystem.current.UpdateModules(); // 이벤트 시스템 강제 업데이트
    }



    public void HideLevelUpOptions()
    {
        if (selectionPanel != null)
        {
            selectionPanel.SetActive(false);
            Debug.Log("레벨업 선택창 숨기기");
        }
    }



    // 최대 HP 증가 선택
    void OnIncreaseMaxHPSelected()
    {
        Debug.Log("최대 HP 증가 선택");
        playerXPManager.IncreaseMaxHP();
        HideLevelUpOptions();
    }

    // 공격력 증가 선택
    void OnIncreaseAttackPowerSelected()
    {
        Debug.Log("공격력 증가 선택");
        playerXPManager.IncreaseAttackPower();
        HideLevelUpOptions();
    }

    // 발사 속도 증가 선택
    void OnIncreaseBulletCountSelected()
    {
        Debug.Log("발사 속도 증가 선택");
        playerXPManager.IncreaseBulletCount();
        HideLevelUpOptions();
    }

    void OnIncreaseChainsawSelected()
    {
        Debug.Log("전기톱 개수 추가 선택");
        chainsawCircle.ActivateNextChainsaw();
        //playerXPManager.IncreaseBulletCount();
        HideLevelUpOptions();
    }

    void OnBombardSelected()
    {
       // Debug.Log("전기톱 개수 추가 선택");
       // chainsawCircle.ActivateNextChainsaw();
        //playerXPManager.IncreaseBulletCount();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 pos = player.GetComponent<Transform>().position;
        missileBombard.FireMissiles(pos);
        HideLevelUpOptions();
    }


}