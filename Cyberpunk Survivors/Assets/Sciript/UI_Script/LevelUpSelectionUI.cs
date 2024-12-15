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

    //��ų ������Ʈ�� �߰�
    private NormalGun gunScript;

    void Awake()
    {
        // selectionPanel�� ����� �����Ǿ����� Ȯ���ϰ� �ʱ�ȭ
        if (selectionPanel == null)
        {
            Debug.LogError("selectionPanel�� null �����Դϴ�. UI�� Ȯ���ϼ���.");
        }
        else
        {
            selectionPanel.SetActive(false); // �⺻������ ��Ȱ��ȭ
            Debug.Log("selectionPanel �ʱ�ȭ �Ϸ�.");
        }

        // ��ư Ȯ�� �� �ʱ�ȭ
        if (increaseMaxHPButton != null)
            increaseMaxHPButton.onClick.AddListener(OnIncreaseMaxHPSelected);
        else
            Debug.LogError("IncreaseMaxHPButton�� ������� �ʾҽ��ϴ�!");

        if (increaseAttackPowerButton != null)
            increaseAttackPowerButton.onClick.AddListener(OnIncreaseAttackPowerSelected);
        else
            Debug.LogError("IncreaseAttackPowerButton�� ������� �ʾҽ��ϴ�!");

        if (increaseBulletCountButton != null)
            increaseBulletCountButton.onClick.AddListener(OnIncreaseBulletCountSelected);
        else
            Debug.LogError("IncreaseBulletCountButton�� ������� �ʾҽ��ϴ�!");

        if (increaseChainsawButton != null)
            increaseChainsawButton.onClick.AddListener(OnIncreaseChainsawSelected);
        else
            Debug.LogError("OnIncreaseChainsawSelected ������� �ʾҽ��ϴ�!");

        if (BombardButton != null)
            BombardButton.onClick.AddListener(OnBombardSelected);
        else
            Debug.LogError("BombardButton ������� �ʾҽ��ϴ�!");
    }


    void Start()
    {

        gunScript = GetComponentInParent<Player>().GetComponentInChildren<NormalGun>();
        if (gunScript == null)
        {
            Debug.LogError("Gun ��ũ��Ʈ�� ã�� �� �����ϴ�!");
        }
        chainsawCircle = GameObject.FindGameObjectWithTag("ChainsawCircle").GetComponent<ChainsawCircle>();
        if (chainsawCircle == null)
        {
            Debug.LogError("chainsawCircle ������� �ʾҽ��ϴ�.");
        }

        missileBombard = GameObject.FindGameObjectWithTag("MissileBombard").GetComponent<MissileBombard>();
        if (chainsawCircle == null)
        {
            Debug.LogError("chainsawCircle ������� �ʾҽ��ϴ�.");
        }

        if (playerXPManager == null)
        {
            Debug.LogError("PlayerXPManager�� ������� �ʾҽ��ϴ�.");
        }
        if (increaseMaxHPButton == null)
            Debug.LogError("IncreaseMaxHPButton�� ������� �ʾҽ��ϴ�!");
        else
            Debug.Log("IncreaseMaxHPButton �����.");

        if (increaseAttackPowerButton == null)
            Debug.LogError("IncreaseAttackPowerButton�� ������� �ʾҽ��ϴ�!");
        else
            Debug.Log("IncreaseAttackPowerButton �����.");

        if (increaseBulletCountButton == null)
            Debug.LogError("IncreaseBulletCountButton�� ������� �ʾҽ��ϴ�!");
        else
            Debug.Log("IncreaseBulletCountButton �����.");
        
        if (increaseChainsawButton == null)
            Debug.LogError("increaseChainsawButton ������� �ʾҽ��ϴ�!");
        else
            Debug.Log("increaseChainsawButton �����.");
        
        if (BombardButton == null)
            Debug.LogError("BombardButton ������� �ʾҽ��ϴ�!");
        else
            Debug.Log("BombardButton �����.");

        // ��ư�� Ŭ�� �̺�Ʈ �Ҵ�
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
            selectionPanel.SetActive(true); // ����â Ȱ��ȭ
            Debug.Log("������ ����â ǥ��");
        }
        else
        {
            Debug.LogError("������ ����â�� ǥ���Ϸ� ������ selectionPanel�� null �����Դϴ�.");
        }

        // Time.timeScale�� 0�� ��� ��ư �Է� Ȱ��ȭ
        EventSystem.current.UpdateModules(); // �̺�Ʈ �ý��� ���� ������Ʈ
    }



    public void HideLevelUpOptions()
    {
        if (selectionPanel != null)
        {
            selectionPanel.SetActive(false);
            Debug.Log("������ ����â �����");
        }
    }



    // �ִ� HP ���� ����
    void OnIncreaseMaxHPSelected()
    {
        Debug.Log("�ִ� HP ���� ����");
        playerXPManager.IncreaseMaxHP();
        HideLevelUpOptions();
    }

    // ���ݷ� ���� ����
    void OnIncreaseAttackPowerSelected()
    {
        Debug.Log("���ݷ� ���� ����");
        playerXPManager.IncreaseAttackPower();
        HideLevelUpOptions();
    }

    // �߻� �ӵ� ���� ����
    void OnIncreaseBulletCountSelected()
    {
        Debug.Log("�߻� �ӵ� ���� ����");
        playerXPManager.IncreaseBulletCount();
        HideLevelUpOptions();
    }

    void OnIncreaseChainsawSelected()
    {
        Debug.Log("������ ���� �߰� ����");
        chainsawCircle.ActivateNextChainsaw();
        //playerXPManager.IncreaseBulletCount();
        HideLevelUpOptions();
    }

    void OnBombardSelected()
    {
       // Debug.Log("������ ���� �߰� ����");
       // chainsawCircle.ActivateNextChainsaw();
        //playerXPManager.IncreaseBulletCount();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 pos = player.GetComponent<Transform>().position;
        missileBombard.FireMissiles(pos);
        HideLevelUpOptions();
    }


}