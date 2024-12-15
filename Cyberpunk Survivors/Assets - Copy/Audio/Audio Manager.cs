using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name; // 오디오 이름
        public AudioClip clip; // 오디오 클립
        [Range(0f, 1f)] public float volume = 0.5f; // 볼륨
        [Range(0.1f, 3f)] public float pitch = 1f; // 피치
        public bool loop; // 반복 여부

        [HideInInspector] public AudioSource source; // AudioSource 참조
    }

    public List<Sound> sounds = new List<Sound>(); // 사운드 리스트
    [Range(0f, 1f)] public float masterVolume = 0.5f; // 전체 볼륨 (조정 가능)

    void Awake()
    {
        // Singleton 패턴
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

        // Resources 폴더에서 오디오 로드
        LoadAudioClips();

        // AudioSource 초기화
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
        // Resources/Audio 폴더에서 모든 오디오 클립 로드
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

        foreach (AudioClip clip in clips)
        {
            // 이름 기반으로 Sound 객체 생성
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
        loop(); //배경음악 루프
    }
    public void loop()//배경음악 루프
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
 
  
    public void PlayOnceAtATime(string name)   //플레이 중 아닐 때만 재생
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
        masterVolume = Mathf.Clamp(volume, 0f, 1f); // 0 ~ 1 사이로 제한
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * masterVolume; // 각 사운드의 볼륨 업데이트
        }
    }

    void OnGUI()
    {
        // GUI 슬라이더를 통해 볼륨 조정
        GUILayout.Label("Master Volume");
        masterVolume = GUILayout.HorizontalSlider(masterVolume, 0f, 1f);

        // 실시간으로 볼륨 반영
        SetMasterVolume(masterVolume);
    }
}
