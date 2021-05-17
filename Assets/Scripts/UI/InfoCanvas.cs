using TMPro;
using UnityEngine;

/// <summary>
/// Final Result canvas
/// </summary>
public class InfoCanvas : UICanvas
{
    [SerializeField]
    private PlayerDataScriptable _playerData;

    [SerializeField]
    private TextMeshProUGUI _pointsText;

    [SerializeField]
    private TextMeshProUGUI _timeText;

    public override void ActiveCanvas()
    {
        base.ActiveCanvas();
        _pointsText.text = $"Points: {_playerData.Points}";
        _timeText.text = $"Time: { _playerData.Time:0.0} s";
    }
}
