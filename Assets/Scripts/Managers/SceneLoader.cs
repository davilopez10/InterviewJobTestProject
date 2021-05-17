using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls scene changes
/// @TODO: Do load level async
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onStartLevel;

    /// <summary>
    /// Load level by index
    /// </summary>
    /// <param name="level"></param>
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    /// <summary>
    /// Application Quit
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }


    private void Start()
    {
        _onStartLevel?.Invoke();
    }
}
