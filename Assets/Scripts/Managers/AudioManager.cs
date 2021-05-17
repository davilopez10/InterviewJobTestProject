using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Manages to play music and audio effect
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Information to play a clip
    /// </summary>
    [System.Serializable]
    public struct AudioClipInfo
    {
        public string name;
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        public float randomPitch;
    }

    public enum AudioTypes
    {
        Music,
        Effect
    };

    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private AudioListScriptable _audioList;

    [SerializeField]
    private AudioSource[] _audioSources;

    [SerializeField, Tooltip("Speed when pitch change")]
    private float _interpolatePitchSpeed = 2f;

    [SerializeField]
    private string _playMusicOnAwake;

    private AudioSource _currentMainThemePlaying;

    private Coroutine _interpolatePitchCoroutine;

    public void ChangePitchMainTheme(float value)
    {
        if (_interpolatePitchCoroutine != null)
        {
            StopCoroutine(_interpolatePitchCoroutine);
        }

        _interpolatePitchCoroutine = StartCoroutine(InterpolatePitch(value));

    }

    public void PlayMusic(string themeName)
    {
        AudioClipInfo infoClip = _audioList.GetAudioByName(themeName, AudioTypes.Music);

        if (infoClip.clip != null)
        {
            if (_currentMainThemePlaying != null)
            {
                _currentMainThemePlaying.Stop();
            }

            AudioSource audioSourceFree = GetFreeAudioSource();

            if (audioSourceFree != null)
            {
                audioSourceFree.clip = infoClip.clip;
                audioSourceFree.outputAudioMixerGroup = infoClip.mixerGroup;
                audioSourceFree.loop = true;
                audioSourceFree.Play();
            }

            _currentMainThemePlaying = audioSourceFree;
        }
    }

    public void PlayEffect(string effectName)
    {
        AudioClipInfo infoClip = _audioList.GetAudioByName(effectName, AudioTypes.Effect);

        if (infoClip.clip != null)
        {
            AudioSource audioSourceFree = GetFreeAudioSource();

            if (audioSourceFree != null)
            {
                audioSourceFree.clip = infoClip.clip;
                audioSourceFree.outputAudioMixerGroup = infoClip.mixerGroup;

                if (infoClip.randomPitch > 0)
                {
                    audioSourceFree.pitch += Random.Range(-infoClip.randomPitch, infoClip.randomPitch);
                }

                audioSourceFree.Play();
                StartCoroutine(StopAudioAtFinish(audioSourceFree));
            }
        }
    }

    private AudioSource GetFreeAudioSource()
    {
        for (int i = 0; i < _audioSources.Length; i++)
        {
            if (_audioSources[i].isPlaying == false)
            {
                return _audioSources[i];
            }
        }

#if UNITY_EDITOR
        Debug.LogWarning("[AudioManager] No audiosource free");
#endif

        return null;
    }

    private IEnumerator InterpolatePitch(float value)
    {
        float currentValue = _currentMainThemePlaying.pitch;
        float t = 0;

        while (t < 1)
        {
            _currentMainThemePlaying.pitch = Mathf.Lerp(currentValue, currentValue + value, t);
            t += _interpolatePitchSpeed * Time.deltaTime;
            yield return null;
        }

        _interpolatePitchCoroutine = null;
    }

    private IEnumerator StopAudioAtFinish(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        source.pitch = 1;
        source.Stop();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _audioList.Initialize();

        if (string.IsNullOrEmpty(_playMusicOnAwake) == false)
        {
            PlayMusic(_playMusicOnAwake);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
