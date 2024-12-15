using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name; // ����� �̸�
        public AudioClip clip; // ����� Ŭ��
        [Range(0f, 1f)] public float volume = 0.5f; // ����
        [Range(0.1f, 3f)] public float pitch = 1f; // ��ġ
        public bool loop; // �ݺ� ����

        [HideInInspector] public AudioSource source; // AudioSource ����
    }

    public List<Sound> sounds = new List<Sound>(); // ���� ����Ʈ
    [Range(0f, 1f)] public float masterVolume = 0.5f; // ��ü ���� (���� ����)

    void Awake()
    {
        // Singleton ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Resources �������� ����� �ε�
        LoadAudioClips();

        // AudioSource �ʱ�ȭ
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void LoadAudioClips()
    {
        // Resources/Audio �������� ��� ����� Ŭ�� �ε�
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

        foreach (AudioClip clip in clips)
        {
            // �̸� ������� Sound ��ü ����
            Sound newSound = new Sound
            {
                name = clip.name,
                clip = clip,
                volume = 0.5f,
                pitch = 1f,
                loop = false
            };
            sounds.Add(newSound);
        }
        Debug.Log("Loaded " + sounds.Count + " audio clips.");
        loop(); //������� ����
    }
    public void loop()//������� ����
    {
        sounds.Find(sound => sound.name == "main bgm").loop = true;
        sounds.Find(sound => sound.name == "ambience_rain").loop = true;

    }

    public void Play(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
 
  
    public void PlayOnceAtATime(string name)   //�÷��� �� �ƴ� ���� ���
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void SetVolume(string name, float volume)
    {
        Sound s = sounds.Find(sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = volume;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp(volume, 0f, 1f); // 0 ~ 1 ���̷� ����
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * masterVolume; // �� ������ ���� ������Ʈ
        }
    }

    void OnGUI()
    {
        // GUI �����̴��� ���� ���� ����
        GUILayout.Label("Master Volume");
        masterVolume = GUILayout.HorizontalSlider(masterVolume, 0f, 1f);

        // �ǽð����� ���� �ݿ�
        SetMasterVolume(masterVolume);
    }
}
