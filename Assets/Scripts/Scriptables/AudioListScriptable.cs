using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

/// <summary>
/// Save list of audios for AudioManager
/// </summary>
[CreateAssetMenu(fileName = "AudioList", menuName = "ScriptableObjects/AudioList")]
public class AudioListScriptable : ScriptableObject
{
    [SerializeField]
    private AudioClipInfo[] _mainThemes;

    [SerializeField]
    private AudioClipInfo[] _audioEffects;

    private Dictionary<string, AudioClipInfo> _mainThemesDict = new Dictionary<string, AudioClipInfo>();

    private Dictionary<string, AudioClipInfo> _audioEffectsDict = new Dictionary<string, AudioClipInfo>();

    /// <summary>
    /// Return audio of dict (MainThemes or Effects)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public AudioClipInfo GetAudioByName(string name, AudioManager.AudioTypes type)
    {
        AudioClipInfo info = default;

        switch (type)
        {
            case AudioTypes.Music:
                _mainThemesDict.TryGetValue(name, out info);
                break;

            case AudioTypes.Effect:
                _audioEffectsDict.TryGetValue(name, out info);
                break;
        }

#if UNITY_EDITOR
        if (info.clip == null)
        {
            Debug.LogWarning("[AudioListScriptable] Name not found");
        }
#endif
        return info;
    }

    /// <summary>
    /// Generate dictionaries
    /// </summary>
    public void Initialize()
    {
        //Create main themes dict

        _mainThemesDict.Clear();
        for (int i = 0; i < _mainThemes.Length; i++)
        {
            if (_mainThemesDict.ContainsKey(_mainThemes[i].name) == false)
            {
                _mainThemesDict.Add(_mainThemes[i].name, _mainThemes[i]);
            }
        }

        //Create audio effects dict

        _audioEffectsDict.Clear();
        for (int i = 0; i < _audioEffects.Length; i++)
        {
            if (_audioEffectsDict.ContainsKey(_audioEffects[i].name) == false)
            {
                _audioEffectsDict.Add(_audioEffects[i].name, _audioEffects[i]);
            }
        }
    }
}
