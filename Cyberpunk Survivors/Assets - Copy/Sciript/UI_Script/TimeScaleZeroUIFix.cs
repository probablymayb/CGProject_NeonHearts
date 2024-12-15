using UnityEngine;
using UnityEngine.EventSystems;

public class TimeScaleZeroUIFix : MonoBehaviour
{
    void Update()
    {
        if (Time.timeScale == 0)
        {
            EventSystem.current.UpdateModules(); // 강제로 이벤트 시스템 갱신
        }
    }
}
