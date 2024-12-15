using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ToggleLights))]
public class ToggleLightsEditor : Editor
{
    private SerializedProperty lightsOnProperty;

    private void OnEnable()
    {
        lightsOnProperty = serializedObject.FindProperty("lightsOn");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(lightsOnProperty);
        if (GUI.changed)
        {
            ToggleLights toggleLights = (ToggleLights)target;
            toggleLights.ToggleLightsState();
            EditorUtility.SetDirty(target);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif

public class ToggleLights : MonoBehaviour
{
    [HideInInspector] public bool lightsOn = true; // Set this to true if you want the lights to be initially on

    private void Start()
    {
        // Set the initial state of the lights based on the 'lightsOn' variable
        SetLightsStateRecursive(transform, lightsOn);
    }

    public void ToggleLightsState()
    {
        // Toggle the lights state (On -> Off or Off -> On)
        lightsOn = !lightsOn;
        SetLightsStateRecursive(transform, lightsOn);
    }

    private void SetLightsStateRecursive(Transform currentTransform, bool state)
    {
        // Check if the current object has a Light component attached
        Light light = currentTransform.GetComponent<Light>();
        if (light != null)
        {
            // Enable or disable the light based on the given state
            light.enabled = state;
        }

        // Recursively call the function for all the children
        for (int i = 0; i < currentTransform.childCount; i++)
        {
            SetLightsStateRecursive(currentTransform.GetChild(i), state);
        }
    }
}
