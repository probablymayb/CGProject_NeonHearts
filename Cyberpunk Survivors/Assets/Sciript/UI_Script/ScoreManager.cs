using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int KillScore { get; private set; }
    public float PlayTime { get; private set; }

    private float startTime;

    [SerializeField] private TMPro.TMP_Text killScoreText; // ų ���ھ� UI �ؽ�Ʈ
    [SerializeField] private TMPro.TMP_Text gameTimeText;  // ���� �ð� UI �ؽ�Ʈ

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
        DontDestroyOnLoad(this); // �� ��ȯ �ÿ��� ����
    }

    void Start()
    {
        startTime = Time.time; // ���� ���� �ð� ���
    }

    void Update()
    {
        PlayTime = Time.time - startTime; // �÷��� �ð� ����
        UpdateGameTime();
    }

    public void IncreaseKillScore(int value = 1)
    {
        KillScore += value; // ų ���ھ� ����
        killScoreText.text = $"Score: {KillScore}"; // UI ������Ʈ
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
