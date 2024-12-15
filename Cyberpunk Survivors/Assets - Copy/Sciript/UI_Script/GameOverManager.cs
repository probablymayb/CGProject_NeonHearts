using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timePlayedText;
    [SerializeField] private TMP_Text killScoreText;

    void Start()
    {
        // ScoreManager���� ������ ��������
        float playTime = ScoreManager.Instance.PlayTime;
        int killScore = ScoreManager.Instance.KillScore;

        // �ؽ�Ʈ ������Ʈ
        timePlayedText.text = $"Time Played: {FormatTime(playTime)}";
        killScoreText.text = $"Kill Score: {killScore}";
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene"); // ���� ������ ��ȯ
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartScene"); // ���� ȭ������ ��ȯ
    }
}

