using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string gameSceneName = "map scenes"; // ���� �� �̸�


    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("StartButton�� ������� �ʾҽ��ϴ�.");
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(() =>
            {
                Debug.Log("���� ���� ��ư Ŭ����.");
                GameManager.Instance.QuitGame();
            });
        }
        else
        {
            Debug.LogError("QuitButton�� ������� �ʾҽ��ϴ�.");
        }
    }
    void OnButtonClicked()
    {
       SceneManager.LoadScene("map scenes");
    }

}
