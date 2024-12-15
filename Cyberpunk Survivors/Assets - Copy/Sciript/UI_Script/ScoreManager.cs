using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int KillScore { get; private set; }
    public float PlayTime { get; private set; }

    private float startTime;

    [SerializeField] private TMPro.TMP_Text killScoreText; // 킬 스코어 UI 텍스트
    [SerializeField] private TMPro.TMP_Text gameTimeText;  // 게임 시간 UI 텍스트

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this); // 씬 전환 시에도 유지
    }

    void Start()
    {
        startTime = Time.time; // 게임 시작 시간 기록
    }

    void Update()
    {
        PlayTime = Time.time - startTime; // 플레이 시간 갱신
        UpdateGameTime();
    }

    public void IncreaseKillScore(int value = 1)
    {
        KillScore += value; // 킬 스코어 증가
        killScoreText.text = $"Score: {KillScore}"; // UI 업데이트
    }

    private void UpdateGameTime()
    {
        if (gameTimeText != null)
        {
            int minutes = Mathf.FloorToInt(PlayTime / 60f);
            int seconds = Mathf.FloorToInt(PlayTime % 60f);
            gameTimeText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }
}
