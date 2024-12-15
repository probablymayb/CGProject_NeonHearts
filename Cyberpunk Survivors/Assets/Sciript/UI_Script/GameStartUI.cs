using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string gameSceneName = "map scenes"; // 게임 씬 이름


    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("StartButton이 연결되지 않았습니다.");
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(() =>
            {
                Debug.Log("게임 종료 버튼 클릭됨.");
                GameManager.Instance.QuitGame();
            });
        }
        else
        {
            Debug.LogError("QuitButton이 연결되지 않았습니다.");
        }
    }
    void OnButtonClicked()
    {
       SceneManager.LoadScene("map scenes");
    }

}
