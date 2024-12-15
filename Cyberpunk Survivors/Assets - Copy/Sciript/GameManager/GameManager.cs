using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글톤 인스턴스

    [Header("Game Settings")]
    public bool isPaused = false; // 게임 멈춤 상태
    public int monsterSpawnCount = 1000; // 몬스터 스폰 마릿수
    public float monsterDamageMultiplier = 1.0f; // 몬스터 데미지 배수
    public float monsterDefenseMultiplier = 1.0f; // 몬스터 방어력 배수

    [Header("Difficulty Settings")]
    public float difficultyIncreaseInterval = 180f; // 난이도 증가 주기 (초)
    public float spawnCountMultiplier = 1.5f; // 스폰 수 증가 배율
    public float damageMultiplier = 1.2f; // 몬스터 공격력 증가 배율
    public float defenseMultiplier = 1.2f; // 몬스터 방어력 증가 배율

    private float elapsedTime = 0f; // 경과 시간 추적

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에도 유지
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스 삭제
        }

        Debug.Log("GameManager 초기화 완료.");
        Time.timeScale = 1; // 기본값 설정
    }

    void Update()
    {
        if (isPaused) return; // 게임이 일시정지된 상태에서는 업데이트 중단

        elapsedTime += Time.deltaTime;

        // 난이도 증가 주기에 도달했을 때
        if (elapsedTime >= difficultyIncreaseInterval)
        {
            IncreaseDifficulty();
            elapsedTime = 0f; // 경과 시간 초기화
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        Debug.Log($"게임 상태 변경. isPaused: {isPaused}, Time.timeScale: {Time.timeScale}");
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1; // 씬 로드 시 시간 정지 해제
        SceneManager.LoadScene(sceneName);
    }

    public void StartGame(string gameSceneName)
    {
        Debug.Log($"게임 시작: {gameSceneName}");
        LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료.");
        Application.Quit();
    }

    // 난이도 증가 메서드
    private void IncreaseDifficulty()
    {
        // 스폰 수 증가
        monsterSpawnCount = Mathf.CeilToInt(monsterSpawnCount * spawnCountMultiplier);
        Debug.Log($"난이도 증가! 새로운 몬스터 수: {monsterSpawnCount}");

        // 공격력 증가
        monsterDamageMultiplier *= damageMultiplier;
        Debug.Log($"몬스터 공격력 배수 증가! 새로운 배수: {monsterDamageMultiplier}");

        // 방어력 증가
        monsterDefenseMultiplier *= defenseMultiplier;
        Debug.Log($"몬스터 방어력 배수 증가! 새로운 배수: {monsterDefenseMultiplier}");
    }
}
