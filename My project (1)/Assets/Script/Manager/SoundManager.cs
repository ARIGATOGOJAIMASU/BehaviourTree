using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound { Bgm, Effect, MaxCount}

public class SoundManager : MonoBehaviour
{
    public AudioSource[] audioSources = new AudioSource[(int)Sound.MaxCount];
    Dictionary<string, AudioClip> audioClips = new();

    private static SoundManager _instance;

    public static SoundManager Instance()
    {
        if(_instance == null)
        {
            _instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;
        }

        return _instance;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        Object.DontDestroyOnLoad(transform);

        string[] soundNames = System.Enum.GetNames(typeof(Sound));

        for(int i = 0; i < soundNames.Length - 1; ++i)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = transform;
            Debug.Log(go);
        }

        Play("BGM",Sound.Bgm, 0.8f);
        audioSources[(int)Sound.Bgm].loop = true;
    }

    public void Clear()
    {
        for(int i = 0; i < (int)Sound.MaxCount; ++i)
        {
            audioSources[i].clip = null;
            audioSources[i].Stop();
        }

        audioClips.Clear();
    }

    AudioClip GetOrAddAudioClip(string path, Sound type = Sound.Effect)
    {
        if (path.Contains("Sound/") == false)
            path = $"Sound/{path}"; // 📂Sound 폴더 안에 저장될 수 있도록

        AudioClip audioClip = null;

        if (!audioClips.ContainsKey(path))
        {
            if (type == Sound.Bgm) // BGM 배경음악 클립 붙이기
            {
                audioClip = Resources.Load<AudioClip>(path);
            }
            else // Effect 효과음 클립 붙이기
            {
                if (audioClips.TryGetValue(path, out audioClip) == false)
                {
                    audioClip = Resources.Load<AudioClip>(path);
                    audioClips.Add(path, audioClip);
                }
            }
        }
        else
        {
            audioClip = audioClips[path];
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }

    public void Play(string path, Sound type = Sound.Effect, float pitch = 1.0f, float StartTime = 0)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch, StartTime);
    }

    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f, float StartTime = 0)
    {
        if (audioClip == null) return;

        if (type == Sound.Bgm)
        {
            AudioSource audioSource = audioSources[(int)Sound.Bgm];

            if(audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = audioSources[(int)Sound.Effect];
            audioSource.pitch = pitch;

            audioSource.PlayOneShot(audioClip);
        }
    }
}
