using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timePlayedText;
    [SerializeField] private TMP_Text killScoreText;

    void Start()
    {
        // ScoreManager에서 데이터 가져오기
        float playTime = ScoreManager.Instance.PlayTime;
        int killScore = ScoreManager.Instance.KillScore;

        // 텍스트 업데이트
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
        SceneManager.LoadScene("GameScene"); // 게임 씬으로 전환
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartScene"); // 메인 화면으로 전환
    }
}

