using UnityEngine;

/// <summary>
/// Execute audio events in animations
/// </summary>
public class AnimatorLaunchAudioEvents : MonoBehaviour
{
    public void ExecuteAudioEvent(string audioEffect)
    {
        AudioManager.Instance.PlayEffect(audioEffect);
    }
}
