using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // �̱��� �ν��Ͻ�

    [Header("Game Settings")]
    public bool isPaused = false; // ���� ���� ����
    public int monsterSpawnCount = 1000; // ���� ���� ������
    public float monsterDamageMultiplier = 1.0f; // ���� ������ ���
    public float monsterDefenseMultiplier = 1.0f; // ���� ���� ���

    [Header("Difficulty Settings")]
    public float difficultyIncreaseInterval = 180f; // ���̵� ���� �ֱ� (��)
    public float spawnCountMultiplier = 1.5f; // ���� �� ���� ����
    public float damageMultiplier = 1.2f; // ���� ���ݷ� ���� ����
    public float defenseMultiplier = 1.2f; // ���� ���� ���� ����

    private float elapsedTime = 0f; // ��� �ð� ����

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ���� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ��� �ν��Ͻ� ����
        }

        Debug.Log("GameManager �ʱ�ȭ �Ϸ�.");
        Time.timeScale = 1; // �⺻�� ����
    }

    void Update()
    {
        if (isPaused) return; // ������ �Ͻ������� ���¿����� ������Ʈ �ߴ�

        elapsedTime += Time.deltaTime;

        // ���̵� ���� �ֱ⿡ �������� ��
        if (elapsedTime >= difficultyIncreaseInterval)
        {
            IncreaseDifficulty();
            elapsedTime = 0f; // ��� �ð� �ʱ�ȭ
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        Debug.Log($"���� ���� ����. isPaused: {isPaused}, Time.timeScale: {Time.timeScale}");
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1; // �� �ε� �� �ð� ���� ����
        SceneManager.LoadScene(sceneName);
    }

    public void StartGame(string gameSceneName)
    {
        Debug.Log($"���� ����: {gameSceneName}");
        LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("���� ����.");
        Application.Quit();
    }

    // ���̵� ���� �޼���
    private void IncreaseDifficulty()
    {
        // ���� �� ����
        monsterSpawnCount = Mathf.CeilToInt(monsterSpawnCount * spawnCountMultiplier);
        Debug.Log($"���̵� ����! ���ο� ���� ��: {monsterSpawnCount}");

        // ���ݷ� ����
        monsterDamageMultiplier *= damageMultiplier;
        Debug.Log($"���� ���ݷ� ��� ����! ���ο� ���: {monsterDamageMultiplier}");

        // ���� ����
        monsterDefenseMultiplier *= defenseMultiplier;
        Debug.Log($"���� ���� ��� ����! ���ο� ���: {monsterDefenseMultiplier}");
    }
}
