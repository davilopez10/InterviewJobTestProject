using UnityEngine;

/// <summary>
/// Base class for UI Canvases
/// </summary>
public class UICanvas : MonoBehaviour
{
    [SerializeField]
    protected Canvas canvas;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public CanvasGroup CanvasGroup { get => canvasGroup; }

    public virtual void ActiveCanvas()
    {
        canvas.enabled = true;
    }

    public virtual void DisableCanvas()
    {
        canvas.enabled = false;
    }
}
