using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage all canvas
/// @TODO: Alpha animation to Show and Hide
/// </summary>
public class CanvasManager : MonoBehaviour
{
    // Should to have same name as gameobject
    public const string POINTS_CANVAS = "PointsCanvas";
    public const string WIN_CANVAS = "WinCanvas";
    public const string GAMEOVER_CANVAS = "GameOverCanvas";

    private Dictionary<string, UICanvas> _canvas = new Dictionary<string, UICanvas>();

    private UICanvas _currentCanvasActive = null;

    public void HideActiveCanvas()
    {
        if (_currentCanvasActive != null)
        {
            HideCanvas(_currentCanvasActive.CanvasGroup);
            _currentCanvasActive.DisableCanvas();
            _currentCanvasActive = null;
        }
    }

    public void ShowCanvas(string name)
    {
        HideActiveCanvas();

        if (_canvas.TryGetValue(name, out UICanvas value))
        {
            ShowCanvas(value.CanvasGroup);
            value.ActiveCanvas();
        }
    }

    private void HideCanvas(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    private void ShowCanvas(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    private void Awake()
    {
        UICanvas[] uiCanvas = GetComponentsInChildren<UICanvas>(true);

        for (int i = 0; i < uiCanvas.Length; i++)
        {
            _canvas.Add(uiCanvas[i].name, uiCanvas[i]);
        }
    }
}
