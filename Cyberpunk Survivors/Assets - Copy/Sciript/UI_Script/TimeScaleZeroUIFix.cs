using UnityEngine;
using UnityEngine.EventSystems;

public class TimeScaleZeroUIFix : MonoBehaviour
{
    void Update()
    {
        if (Time.timeScale == 0)
        {
            EventSystem.current.UpdateModules(); // ������ �̺�Ʈ �ý��� ����
        }
    }
}
